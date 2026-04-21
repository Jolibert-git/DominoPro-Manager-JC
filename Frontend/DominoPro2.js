'use strict';
 
const API_BASE = 'https://localhost:7231/Api';
 
const api = {
  async get(endpoint) {
    const res = await fetch(`${API_BASE}${endpoint}`);
    if (!res.ok) throw new Error(`Error ${res.status}: ${res.statusText}`);
    return res.json();
  },
 
  async post(endpoint, body) {
    const res = await fetch(`${API_BASE}${endpoint}`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(body),
    });
    const data = await res.json();
    if (!res.ok) throw new ApiError(data.message || `Error ${res.status} al crear recurso`, res.status);
    return data;
  },
 
  async put(endpoint, body) {
    const res = await fetch(`${API_BASE}${endpoint}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(body),
    });
    const data = await res.json();
    if (!res.ok) throw new ApiError(data.message || `Error ${res.status} al actualizar recurso`, res.status);
    return data;
  },
 
  async patch(endpoint, body) {
    const res = await fetch(`${API_BASE}${endpoint}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(body),
    });
    const data = await res.json();
    if (!res.ok) throw new ApiError(data.message || `Error ${res.status} al actualizar recurso`, res.status);
    return data;
  },
 
  async delete(endpoint) {
    const res = await fetch(`${API_BASE}${endpoint}`, { method: 'DELETE' });
    const data = await res.json();
    if (!res.ok) throw new ApiError(data.message || `Error ${res.status} al eliminar recurso`, res.status);
    return data;
  },
};
 
class ApiError extends Error {
  constructor(message, status) {
    super(message);
    this.status = status;
  }
}
 
/* ============================================================
   TOASTS
   ============================================================ */
const Toast = {
  container: null,
  init() { this.container = document.getElementById('toast-container'); },
  show(message, type = 'info', duration = 4000) {
    const icons = { success: '✅', error: '❌', warning: '⚠️', info: 'ℹ️' };
    const toast = document.createElement('div');
    toast.className = `toast toast--${type}`;
    toast.innerHTML = `
      <span class="toast__icon">${icons[type]}</span>
      <span class="toast__message">${message}</span>
      <button class="toast__close" aria-label="Cerrar notificación">✕</button>
    `;
    toast.querySelector('.toast__close').addEventListener('click', () => this.remove(toast));
    this.container.appendChild(toast);
    setTimeout(() => this.remove(toast), duration);
  },
  remove(toast) {
    toast.style.opacity = '0';
    toast.style.transform = 'translateX(20px)';
    toast.style.transition = 'all 0.3s ease';
    setTimeout(() => toast.remove(), 300);
  },
  success(msg) { this.show(msg, 'success'); },
  error(msg)   { this.show(msg, 'error', 6000); },
  warning(msg) { this.show(msg, 'warning'); },
  info(msg)    { this.show(msg, 'info'); },
};
 
/* ============================================================
   MODALES
   ============================================================ */
const Modal = {
  open(id) {
    const modal = document.getElementById(id);
    if (!modal) return;
    modal.showModal?.() || modal.setAttribute('open', '');
  },
  close(id) {
    const modal = document.getElementById(id);
    if (!modal) return;
    modal.close?.() || modal.removeAttribute('open');
  },
  closeAll() {
    document.querySelectorAll('dialog[open]').forEach(m => {
      m.close?.() || m.removeAttribute('open');
    });
  },
  init() {
    document.querySelectorAll('.modal__close, .modal__cancel').forEach(btn => {
      btn.addEventListener('click', () => {
        const modal = btn.closest('dialog');
        if (modal) { modal.close?.() || modal.removeAttribute('open'); }
      });
    });
    document.querySelectorAll('dialog').forEach(dialog => {
      dialog.addEventListener('click', e => {
        if (e.target === dialog) {
          dialog.close?.() || dialog.removeAttribute('open');
        }
      });
    });
  },
};
 
/* ============================================================
   CONFIRMACIÓN
   ============================================================ */
const Confirm = {
  _resolve: null,
  show(message = '¿Estás seguro de que deseas continuar? Esta acción no se puede deshacer.') {
    document.getElementById('modal-confirm-message').textContent = message;
    Modal.open('modal-confirm');
    return new Promise(resolve => { this._resolve = resolve; });
  },
  init() {
    document.getElementById('confirm-ok').addEventListener('click', () => {
      Modal.close('modal-confirm');
      this._resolve?.(true);
    });
    document.getElementById('confirm-cancel').addEventListener('click', () => {
      Modal.close('modal-confirm');
      this._resolve?.(false);
    });
  },
};
 
/* ============================================================
   NAVEGACIÓN SPA
   ============================================================ */
const Nav = {
  current: 'dashboard',
  init() {
    document.querySelectorAll('[data-section]').forEach(el => {
      el.addEventListener('click', e => {
        e.preventDefault();
        const section = el.dataset.section;
        const action  = el.dataset.action;
        this.goTo(section);
        if (action === 'new') this.triggerNew(section);
      });
    });
  },
  goTo(section) {
    document.querySelectorAll('.section').forEach(s => {
      s.classList.remove('active');
      s.hidden = true;
    });
    const target = document.getElementById(`section-${section}`);
    if (!target) return;
    target.hidden = false;
    target.classList.add('active');
    this.current = section;
    document.querySelectorAll('[data-section]').forEach(el => {
      el.classList.toggle('active', el.dataset.section === section);
      if (el.dataset.section === section) el.setAttribute('aria-current', 'page');
      else el.removeAttribute('aria-current');
    });
    Sidebar.close();
    this.loadSection(section);
  },
  loadSection(section) {
    switch (section) {
      case 'dashboard':      Dashboard.load();       break;
      case 'tournaments':    Tournaments.load();     break;
      case 'players':        Players.load();         break;
      case 'registrations':  Registrations.init();   break;
      case 'rounds':         Rounds.init();          break;
      case 'tables':         Tables.init();          break;
      case 'results':        Results.init();         break;
      case 'classification': Classification.init();  break;
    }
  },
  triggerNew(section) {
    switch (section) {
      case 'tournaments': Tournaments.openNew(); break;
      case 'players':     Players.openNew();     break;
    }
  },
};
 
/* ============================================================
   SIDEBAR
   ============================================================ */
const Sidebar = {
  sidebar: null, overlay: null,
  init() {
    this.sidebar = document.getElementById('sidebar');
    this.overlay = document.getElementById('sidebar-overlay');
    document.getElementById('sidebar-toggle').addEventListener('click', () => this.toggle());
    document.getElementById('sidebar-close').addEventListener('click',  () => this.close());
    this.overlay.addEventListener('click', () => this.close());
  },
  toggle() {
    this.sidebar.classList.toggle('open');
    this.overlay.classList.toggle('visible');
    const btn = document.getElementById('sidebar-toggle');
    btn.setAttribute('aria-expanded', this.sidebar.classList.contains('open'));
  },
  open()  { this.sidebar.classList.add('open');    this.overlay.classList.add('visible'); },
  close() { this.sidebar.classList.remove('open'); this.overlay.classList.remove('visible'); },
};
 
/* ============================================================
   RENDER HELPERS
   ============================================================ */
const Render = {
  badge(status) {
    const labels = {
      Programmed: 'Programado', InCourse: 'En Curso', Finalized: 'Finalizado',
      Canceled: 'Cancelado', Pending: 'Pendiente', InPlay: 'En Juego',
      Completed: 'Completada', Cancelled: 'Cancelada', Confirmed: 'Confirmado',
      Withdrawn: 'Retirado', Disqualified: 'Descalificado', Singles: 'Singles',
      Doubles: 'Dobles',
    };
    return `<span class="badge badge--${status}">${labels[status] ?? status}</span>`;
  },
  bool(val) {
    return val ? '<span class="bool-yes">Sí</span>' : '<span class="bool-no">No</span>';
  },
  date(iso) {
    if (!iso) return '<span class="text-muted">—</span>';
    return new Date(iso).toLocaleString('es-DO', {
      day: '2-digit', month: 'short', year: 'numeric',
      hour: '2-digit', minute: '2-digit',
    });
  },
  shortDate(iso) {
    if (!iso) return '—';
    return new Date(iso).toLocaleDateString('es-DO', {
      day: '2-digit', month: 'short', year: 'numeric',
    });
  },
  duration(minutes) {
    if (minutes == null) return '—';
    const m = Math.round(minutes);
    if (m < 60) return `${m} min`;
    return `${Math.floor(m / 60)}h ${m % 60}m`;
  },
  initials(name = '') {
    return name.split(' ').map(w => w[0]).slice(0, 2).join('').toUpperCase() || '??';
  },
  actionBtns(actions) {
    return actions.map(a =>
      `<button class="btn btn--sm btn--${a.style ?? 'secondary'}" data-action="${a.action}" data-id="${a.id}" title="${a.label}">${a.icon ?? ''} ${a.label}</button>`
    ).join(' ');
  },
  empty(colspan, message = 'No hay datos disponibles.') {
    return `<tr><td colspan="${colspan}" class="table__empty">${message}</td></tr>`;
  },
  pos(n) {
    const medals = { 1: '🥇', 2: '🥈', 3: '🥉' };
    return medals[n]
      ? `<span class="pos--${n}">${medals[n]} ${n}</span>`
      : `<span class="pos">${n}</span>`;
  },
};
 
/* ============================================================
   DASHBOARD
   ============================================================ */
const Dashboard = {
  async load() {
    try {
      await Promise.all([
        this.loadStats(),
        this.loadRecentTournaments(),
        this.loadLeaderboard(),
      ]);
    } catch (e) {
      Toast.error('Error al cargar el dashboard');
    }
  },
  async loadStats() {
    try {
      const [activeTournaments, allPlayers, activeRounds] = await Promise.all([
        api.get('/Tournament/Active'),
        api.get('/Players/Active'),
        api.get('/Tournament/Status/InCourse'),
      ]);
      document.getElementById('stat-active-tournaments').textContent = activeTournaments.data?.length ?? 0;
      document.getElementById('stat-total-players').textContent      = allPlayers.data?.length ?? 0;
      document.getElementById('stat-active-rounds').textContent      = activeRounds.data?.length ?? 0;
      document.getElementById('stat-active-tables').textContent      = '—';
    } catch {
      ['stat-active-tournaments','stat-total-players','stat-active-rounds','stat-active-tables']
        .forEach(id => { document.getElementById(id).textContent = 'Error'; });
    }
  },
  async loadRecentTournaments() {
    const tbody = document.getElementById('dashboard-tournaments-body');
    try {
      const res = await api.get('/Tournament/Active');
      const list = res.data ?? [];
      if (!list.length) { tbody.innerHTML = Render.empty(4, 'No hay torneos activos.'); return; }
      tbody.innerHTML = list.slice(0, 5).map(t => `
        <tr>
          <td class="name-cell">${t.name}</td>
          <td>${Render.badge(t.mode)}</td>
          <td>${Render.badge(t.status)}</td>
          <td>${t.totalRegistered ?? 0}${t.maxPlayers ? ` / ${t.maxPlayers}` : ''}</td>
        </tr>
      `).join('');
    } catch {
      tbody.innerHTML = Render.empty(4, 'Error al cargar torneos.');
    }
  },
  async loadLeaderboard() {
    const list = document.getElementById('dashboard-leaderboard');
    try {
      const res = await api.get('/Players/Active');
      const players = (res.data ?? []).sort((a, b) => b.elo - a.elo).slice(0, 8);
      if (!players.length) { list.innerHTML = '<li class="leaderboard__empty">Sin jugadores aún.</li>'; return; }
      list.innerHTML = players.map((p, i) => `
        <li>
          <span class="leaderboard__pos">${i + 1}</span>
          <span class="avatar" style="width:28px;height:28px;font-size:.65rem;" aria-hidden="true">
            ${Render.initials(`${p.name} ${p.lastName}`)}
          </span>
          <span class="leaderboard__name">${p.name} ${p.lastName}</span>
          <span class="leaderboard__elo">${p.elo} ELO</span>
        </li>
      `).join('');
    } catch {
      list.innerHTML = '<li class="leaderboard__empty">Error al cargar ranking.</li>';
    }
  },
};
 
/* ============================================================
   TORNEOS  — FIX #1: body correcto  FIX #2: scroll habilitado
              FIX #3: filtro automático
   ============================================================ */
const Tournaments = {
  list: [],
 
  /* FIX #3: carga todos los torneos al entrar a la sección */
  async load(status = '') {
    const tbody = document.getElementById('tournaments-body');
    tbody.innerHTML = Render.empty(8, 'Cargando...');
    try {
      let res;
      if (status) {
        res = await api.get(`/Tournament/Status/${status}`);
      } else {
        /* Sin filtro: traer activos + todos los demás para mostrar la lista completa */
        res = await api.get('/Tournament/Active');
        /* Si el select dice "Todos" pero queremos incluir finalizados/cancelados,
           podemos hacer múltiples llamadas y unirlas */
        const [prog, inc, fin, can] = await Promise.allSettled([
          api.get('/Tournament/Status/Programmed'),
          api.get('/Tournament/Status/InCourse'),
          api.get('/Tournament/Status/Finalized'),
          api.get('/Tournament/Status/Canceled'),
        ]);
        const all = [
          ...(prog.value?.data ?? []),
          ...(inc.value?.data ?? []),
          ...(fin.value?.data ?? []),
          ...(can.value?.data ?? []),
        ];
        /* deduplicar por id */
        const seen = new Set();
        this.list = all.filter(t => { if (seen.has(t.id)) return false; seen.add(t.id); return true; });
        this.renderTable(this.list);
        return;
      }
      this.list = res.data ?? [];
      this.renderTable(this.list);
    } catch (e) {
      tbody.innerHTML = Render.empty(8, 'Error al cargar torneos.');
      Toast.error(e.message);
    }
  },
 
  renderTable(list) {
    const tbody = document.getElementById('tournaments-body');
    if (!list.length) {
      tbody.innerHTML = Render.empty(8, 'No hay torneos que coincidan con los filtros.');
      return;
    }
    tbody.innerHTML = list.map(t => `
      <tr>
        <td>${t.id}</td>
        <td class="name-cell">${t.name}</td>
        <td>${Render.badge(t.mode)}</td>
        <td>${t.place ?? '—'}</td>
        <td>${Render.shortDate(t.startDate)}</td>
        <td>${t.totalRegistered ?? 0}${t.maxPlayers ? ` / ${t.maxPlayers}` : ''}</td>
        <td>${Render.badge(t.status)}</td>
        <td class="table-actions">
          ${Render.actionBtns([
            { action: 'edit-tournament',          id: t.id, label: 'Editar',   icon: '✏️', style: 'secondary' },
            { action: 'change-tournament-status', id: t.id, label: 'Estado',   icon: '🔄', style: 'secondary' },
            { action: 'view-tournament',          id: t.id, label: 'Ver',      icon: '👁️', style: 'secondary' },
            { action: 'delete-tournament',        id: t.id, label: 'Eliminar', icon: '🗑️', style: 'danger' },
          ])}
        </td>
      </tr>
    `).join('');
    this.bindTableActions();
  },
 
  bindTableActions() {
    document.querySelectorAll('#tournaments-body [data-action]').forEach(btn => {
      btn.addEventListener('click', () => {
        const id = parseInt(btn.dataset.id);
        switch (btn.dataset.action) {
          case 'edit-tournament':           this.openEdit(id);   break;
          case 'change-tournament-status':  this.openStatus(id); break;
          case 'view-tournament':           this.openView(id);   break;
          case 'delete-tournament':         this.delete(id);     break;
        }
      });
    });
  },
 
  openNew() {
    document.getElementById('modal-tournament-title').textContent = 'Nuevo Torneo';
    document.getElementById('form-tournament').reset();
    document.getElementById('tournament-id').value = '';
    Modal.open('modal-tournament');
  },
 
  async openEdit(id) {
    try {
      const res = await api.get(`/Tournament/${id}`);
      const t = res.data;
      document.getElementById('modal-tournament-title').textContent = 'Editar Torneo';
      document.getElementById('tournament-id').value           = t.id;
      document.getElementById('tournament-name').value         = t.name ?? '';
      document.getElementById('tournament-description').value  = t.description ?? '';

      let modeValue = t.mode;

      // Si el API te manda el texto "Singles" o "Doubles" en lugar del número:
      if (modeValue === "Singles") modeValue = "0";
      else if (modeValue === "Doubles" || modeValue === "Dobles") modeValue = "1";
      else if (modeValue !== undefined && modeValue !== null) modeValue = String(modeValue);
      else modeValue = "";

document.getElementById('tournament-mode').value = modeValue;

      //document.getElementById('tournament-mode').value = t.mode !== undefined ? String(t.mode) : '';//document.getElementById('tournament-mode').value         = t.mode ?? '';
      document.getElementById('tournament-place').value        = t.place ?? '';
      document.getElementById('tournament-prize').value        = t.prize ?? '';
      document.getElementById('tournament-start-date').value   = toDatetimeLocal(t.startDate);
      document.getElementById('tournament-end-date').value     = toDatetimeLocal(t.endDate);
      document.getElementById('tournament-min-players').value  = t.minPlayers ?? 4;
      document.getElementById('tournament-max-players').value  = t.maxPlayers ?? '';
      document.getElementById('tournament-max-rounds').value   = t.maxRound ?? '';
      document.getElementById('tournament-target-point').value = t.targetPoint ?? 100;
      Modal.open('modal-tournament');
    } catch (e) {
      Toast.error('No se pudo cargar el torneo: ' + e.message);
    }
  },
 
  openStatus(id) {
    document.getElementById('tournament-status-id').value = id;
    document.getElementById('tournament-new-status').value = '';
    Modal.open('modal-tournament-status');
  },
 
  async openView(id) {
    try {
      const res = await api.get(`/Tournament/${id}/Full`);
      const t = res.data;
      Toast.info(`Torneo: ${t.name} | ${t.totalRegistered} jugadores | ${t.rondas?.length ?? 0} rondas`);
    } catch (e) {
      Toast.error(e.message);
    }
  },
 
  async delete(id) {
    const ok = await Confirm.show('¿Eliminar este torneo? Esta acción no se puede deshacer.');
    if (!ok) return;
    try {
      await api.delete(`/Tournament/${id}`);
      Toast.success('Torneo eliminado correctamente.');
      this.load();
    } catch (e) {
      Toast.error(e.message);
    }
  },
 
  initForms() {
    /* FIX #1: body con todos los campos correctos y tipos numéricos bien parseados */
    document.getElementById('form-tournament').addEventListener('submit', async e => {
      e.preventDefault();
      const id = document.getElementById('tournament-id').value;
 
      const minPlayers  = parseInt(document.getElementById('tournament-min-players').value);
      const maxPlayersV = document.getElementById('tournament-max-players').value;
      const maxRoundV   = document.getElementById('tournament-max-rounds').value;
      const endDateV    = document.getElementById('tournament-end-date').value;
 
      const body = {
        
        name:        document.getElementById('tournament-name').value.trim(),
        description: document.getElementById('tournament-description').value.trim() || null,
        mode:        parseInt(document.getElementById('tournament-mode').value),//document.getElementById('tournament-mode').value,
        place:       document.getElementById('tournament-place').value.trim() || null,
        prize:       document.getElementById('tournament-prize').value.trim() || null,
        startDate:   document.getElementById('tournament-start-date').value 
                 ? new Date(document.getElementById('tournament-start-date').value).toISOString() 
                 : null,//document.getElementById('tournament-start-date').value,
        endDate:     document.getElementById('tournament-end-date').value 
                 ? new Date(document.getElementById('tournament-end-date').value).toISOString() 
                 : null,//endDateV || null,
        minPlayers:  isNaN(minPlayers) ? 4 : minPlayers,
        maxPlayers:  maxPlayersV ? parseInt(maxPlayersV) : null,
        maxRound:    maxRoundV   ? parseInt(maxRoundV)   : null,
        targetPoint: parseInt(document.getElementById('tournament-target-point').value) || 100,
       
      };
 
      /* Validaciones mínimas del lado cliente */
      if (!body.name) { Toast.warning('El nombre del torneo es requerido.'); return; }
      if (isNaN(body.mode)) { Toast.warning('Selecciona el modo del torneo.'); return; }
      if (!body.startDate) { Toast.warning('La fecha de inicio es requerida.'); return; }
 
      try {
        if (id) {
          await api.put(`/Tournament/${id}`, body);
          Toast.success('Torneo actualizado.');
        } else {
          await api.post('/Tournament', body);
          Toast.success('Torneo creado.');
        }
        Modal.close('modal-tournament');
        this.load();
      } catch (e) {
        Toast.error('Error: ' + e.message);
      }
    });
 
    document.getElementById('form-tournament-status').addEventListener('submit', async e => {
      e.preventDefault();
      const id = document.getElementById('tournament-status-id').value;
       const statusValue = document.getElementById('tournament-new-status').value; // Cambié el nombre para no confundir

       if (!statusValue) { Toast.warning('Selecciona un estado.'); return; }

       try {
               // CAMBIO 1: parseInt para que el API reciba un número, no un string "1"
               // CAMBIO 2: Asegúrate que la propiedad se llame 'status' (minúscula) 
               // o como la espere tu DTO en el backend.
              const body = { status: parseInt(statusValue) };

              await api.patch(`/Tournament/${id}/status`, body); 
    
              Toast.success('Estado actualizado.');
              Modal.close('modal-tournament-status');
              this.load();
        } catch (e) {
            // Si el error viene del servidor, tratamos de mostrar el mensaje real
              const msg = e.response?.data?.message || e.message;
              Toast.error('Error: ' + msg);
        }
      /*
      const id     = document.getElementById('tournament-status-id').value;
      const status = document.getElementById('tournament-new-status').value;
      if (!status) { Toast.warning('Selecciona un estado.'); return; }
      try {
        await api.patch(`/Tournament/${id}/Status`, { status });
        Toast.success('Estado actualizado.');
        Modal.close('modal-tournament-status');
        this.load();
      } catch (e) {
        Toast.error(e.message);
      }
        */
    });
 
    document.getElementById('btn-new-tournament')
      .addEventListener('click', () => this.openNew());
 
    /* FIX #3: filtro automático al cambiar el select */
    document.getElementById('filter-tournament-status').addEventListener('change', () => {
      const status = document.getElementById('filter-tournament-status').value;
      this.load(status);
    });
 
    /* FIX #3: filtro de modo aplicado sobre la lista ya cargada */
    document.getElementById('filter-tournament-mode').addEventListener('change', () => {
      const mode = document.getElementById('filter-tournament-mode').value;
      const filtered = mode
        ? this.list.filter(t => t.mode === mode)
        : this.list;
      this.renderTable(filtered);
    });
 
    /* Mantener el botón Filtrar como respaldo */
    document.getElementById('btn-filter-tournaments').addEventListener('click', () => {
      const status = document.getElementById('filter-tournament-status').value;
      const mode   = document.getElementById('filter-tournament-mode').value;
      this.load(status).then(() => {
        if (mode) this.renderTable(this.list.filter(t => t.mode === mode));
      });
    });
  },
};
 
/* ============================================================
   JUGADORES  — FIX #2: scroll habilitado  FIX #3: filtro auto
   ============================================================ */
const Players = {
  page: 1,
  pageSize: 15,
  list: [],
 
  async load() {
    const tbody = document.getElementById('players-body');
    tbody.innerHTML = Render.empty(10, 'Cargando...');
    try {
      const res = await api.get('/Players');
      this.list = res.data ?? [];
      this.page = 1;
      this.renderPage();
    } catch (e) {
      tbody.innerHTML = Render.empty(10, 'Error al cargar jugadores.');
      Toast.error(e.message);
    }
  },
 
  renderPage() {
    const filtered = this.applyFilters();
    const start  = (this.page - 1) * this.pageSize;
    const sliced = filtered.slice(start, start + this.pageSize);
    const tbody  = document.getElementById('players-body');
 
    if (!sliced.length) {
      tbody.innerHTML = Render.empty(10, 'No se encontraron jugadores.');
    } else {
      tbody.innerHTML = sliced.map(p => `
        <tr>
          <td>${p.id}</td>
          <td class="name-cell">${p.name} ${p.lastName}</td>
          <td>${p.email || '—'}</td>
          <td>${p.phone || '—'}</td>
          <td><strong>${p.elo}</strong></td>
          <td>${p.winGame}</td>
          <td>${p.lostGame}</td>
          <td>${p.winRate?.toFixed(1) ?? 0}%</td>
          <td>${Render.badge(p.isActive ? 'Confirmed' : 'Withdrawn')}</td>
          <td>
            ${Render.actionBtns([
              { action: 'edit-player',   id: p.id, label: 'Editar',     icon: '✏️', style: 'secondary' },
              { action: 'stats-player',  id: p.id, label: 'Stats',      icon: '📊', style: 'secondary' },
              { action: 'delete-player', id: p.id, label: 'Desactivar', icon: '🚫', style: 'danger' },
            ])}
          </td>
        </tr>
      `).join('');
    }
 
    const totalPages = Math.ceil(filtered.length / this.pageSize) || 1;
    document.getElementById('players-page-info').textContent = `Página ${this.page} de ${totalPages}`;
    document.getElementById('players-prev').disabled = this.page <= 1;
    document.getElementById('players-next').disabled = this.page >= totalPages;
 
    this.bindTableActions();
  },
 
  applyFilters() {
    const search = document.getElementById('search-player').value.toLowerCase();
    const status = document.getElementById('filter-player-status').value;
    const eloMin = parseInt(document.getElementById('filter-elo-min').value) || 0;
    const eloMax = parseInt(document.getElementById('filter-elo-max').value) || 3000;
 
    return this.list.filter(p => {
      const fullName    = `${p.name} ${p.lastName} ${p.email}`.toLowerCase();
      const matchSearch = !search || fullName.includes(search);
      const matchStatus = status === '' || String(p.isActive) === status;
      const matchElo    = p.elo >= eloMin && p.elo <= eloMax;
      return matchSearch && matchStatus && matchElo;
    });
  },
 
  bindTableActions() {
    document.querySelectorAll('#players-body [data-action]').forEach(btn => {
      btn.addEventListener('click', () => {
        const id = parseInt(btn.dataset.id);
        switch (btn.dataset.action) {
          case 'edit-player':   this.openEdit(id);   break;
          case 'stats-player':  this.openStats(id);  break;
          case 'delete-player': this.deactivate(id); break;
        }
      });
    });
  },
 
  openNew() {
    document.getElementById('modal-player-title').textContent = 'Nuevo Jugador';
    document.getElementById('form-player').reset();
    document.getElementById('player-id').value = '';
    Modal.open('modal-player');
  },
 
  async openEdit(id) {
    try {
      const res = await api.get(`/Players/${id}`);
      const p = res.data;
      document.getElementById('modal-player-title').textContent = 'Editar Jugador';
      document.getElementById('player-id').value       = p.id;
      document.getElementById('player-name').value     = p.name ?? '';
      document.getElementById('player-lastname').value = p.lastName ?? '';
      document.getElementById('player-email').value    = p.email ?? '';
      document.getElementById('player-phone').value    = p.phone ?? '';
      Modal.open('modal-player');
    } catch (e) {
      Toast.error('No se pudo cargar el jugador: ' + e.message);
    }
  },
 
  async openStats(id) {
    try {
      const res = await api.get(`/Players/${id}/Stats`);
      const p   = res.data;
      const ini = Render.initials(`${p.name} ${p.lastName}`);
      document.getElementById('stats-avatar').textContent   = ini;
      document.getElementById('stats-fullname').textContent = `${p.name} ${p.lastName}`;
      document.getElementById('stats-email').textContent    = p.email || '—';
      document.getElementById('stats-elo').textContent      = p.elo;
      document.getElementById('stats-wins').textContent     = p.winGame;
      document.getElementById('stats-losses').textContent   = p.lostGame;
      document.getElementById('stats-winrate').textContent  = `${p.winRate?.toFixed(1) ?? 0}%`;
      document.getElementById('modal-player-stats-title').textContent = `${p.name} ${p.lastName}`;
      Modal.open('modal-player-stats');
    } catch (e) {
      Toast.error(e.message);
    }
  },
 
  async deactivate(id) {
    const ok = await Confirm.show('¿Desactivar este jugador? No podrá participar en torneos.');
    if (!ok) return;
    try {
      await api.delete(`/Players/${id}`);
      Toast.success('Jugador desactivado.');
      this.load();
    } catch (e) {
      Toast.error(e.message);
    }
  },
 
  initForms() {
    document.getElementById('form-player').addEventListener('submit', async e => {
      e.preventDefault();
      const id = document.getElementById('player-id').value;
      const body = {
        name:     document.getElementById('player-name').value.trim(),
        lastName: document.getElementById('player-lastname').value.trim(),
        email:    document.getElementById('player-email').value.trim(),
        phone:    document.getElementById('player-phone').value.trim(),
      };
      try {
        if (id) {
          await api.put(`/Players/${id}`, body);
          Toast.success('Jugador actualizado.');
        } else {
          await api.post('/Players', body);
          Toast.success('Jugador registrado.');
        }
        Modal.close('modal-player');
        this.load();
      } catch (e) {
        Toast.error(e.message);
      }
    });
 
    document.getElementById('btn-new-player')
      .addEventListener('click', () => this.openNew());
 
    /* FIX #3: búsqueda y filtros automáticos */
    document.getElementById('search-player')
      .addEventListener('input', () => { this.page = 1; this.renderPage(); });
 
    document.getElementById('filter-player-status')
      .addEventListener('change', () => { this.page = 1; this.renderPage(); });
 
    document.getElementById('filter-elo-min')
      .addEventListener('input', () => { this.page = 1; this.renderPage(); });
 
    document.getElementById('filter-elo-max')
      .addEventListener('input', () => { this.page = 1; this.renderPage(); });
 
    /* Mantener botón Filtrar como respaldo */
    document.getElementById('btn-filter-players')
      .addEventListener('click', () => { this.page = 1; this.renderPage(); });
 
    document.getElementById('players-prev').addEventListener('click', () => {
      if (this.page > 1) { this.page--; this.renderPage(); }
    });
    document.getElementById('players-next').addEventListener('click', () => {
      const filtered   = this.applyFilters();
      const totalPages = Math.ceil(filtered.length / this.pageSize) || 1;
      if (this.page < totalPages) { this.page++; this.renderPage(); }
    });
  },
};
 
/* ============================================================
   INSCRIPCIONES  — FIX #3: carga automática al seleccionar torneo
   ============================================================ */
   /* ============================================================
   INSCRIPCIONES
   ============================================================ */
const Registrations = {
  async init() {
    await this.populateTournamentSelects();
  },

  async populateTournamentSelects() {
    try {
      const [active, prog] = await Promise.allSettled([
        api.get('/Tournament/Active'),
        api.get('/Tournament/Status/Programmed'),
      ]);
      const tournaments = [
        ...(active.value?.data ?? []),
        ...(prog.value?.data ?? []),
      ];
      const seen = new Set();
      const unique = tournaments.filter(t => {
        if (seen.has(t.id)) return false;
        seen.add(t.id);
        return true;
      });

      ['filter-registration-tournament', 'reg-tournament'].forEach(id => {
        const sel = document.getElementById(id);
        if (!sel) return;
        const current = sel.value;
        sel.innerHTML =
          '<option value="">Seleccionar torneo...</option>' +
          unique.map(t => `<option value="${t.id}">${t.name}</option>`).join('');
        sel.value = current;
      });
    } catch (e) {
      Toast.error('Error al cargar torneos: ' + e.message);
    }
  },

  async loadByTournament(tournamentId) {
    const tbody = document.getElementById('registrations-body');
    if (!tournamentId) {
      tbody.innerHTML = Render.empty(7, 'Selecciona un torneo para ver las inscripciones.');
      return;
    }
    tbody.innerHTML = Render.empty(7, 'Cargando...');
    try {
      const res  = await api.get(`/TournamentRegistration/Tournament/${tournamentId}`);
      const list = res.data ?? [];
      if (!list.length) {
        tbody.innerHTML = Render.empty(7, 'No hay inscripciones para este torneo.');
        return;
      }
      tbody.innerHTML = list.map(r => `
        <tr>
          <td>${r.id}</td>
          <td class="name-cell">${r.player ? r.player.fullName : `Jugador #${r.playerId}`}</td>
          <td>${r.dorsalNumber ?? '—'}</td>
          <td>${Render.badge(r.status)}</td>
          <td>${Render.bool(r.paymentFee)}</td>
          <td>${Render.shortDate(r.registrationDate)}</td>
          <td>
            ${Render.actionBtns([
              { action: 'update-reg-status', id: r.id, label: 'Actualizar', icon: '🔄', style: 'secondary' },
              { action: 'withdraw-reg',      id: r.id, label: 'Retirar',    icon: '🚪', style: 'danger'     },
            ])}
          </td>
        </tr>
      `).join('');
      this.bindTableActions(tournamentId);
    } catch (e) {
      tbody.innerHTML = Render.empty(7, 'Error al cargar inscripciones.');
      Toast.error(e.message);
    }
  },

  bindTableActions(tournamentId) {
    document.querySelectorAll('#registrations-body [data-action]').forEach(btn => {
      btn.addEventListener('click', () => {
        const id = parseInt(btn.dataset.id);
        switch (btn.dataset.action) {
          case 'update-reg-status': this.openUpdateStatus(id);       break;
          case 'withdraw-reg':      this.withdraw(id, tournamentId); break;
        }
      });
    });
  },

  openUpdateStatus(id) {
    document.getElementById('reg-status-id').value        = id;
    document.getElementById('reg-new-status').value       = '0'; // Pending por defecto
    document.getElementById('reg-status-payment').checked = false;
    Modal.open('modal-registration-status');
  },

  async withdraw(id, tournamentId) {
    const ok = await Confirm.show('¿Retirar a este jugador del torneo?');
    if (!ok) return;
    try {
      await api.patch(`/TournamentRegistration/${id}/Withdraw`, {});
      Toast.success('Jugador retirado del torneo.');
      this.loadByTournament(tournamentId);
    } catch (e) {
      Toast.error(e.message);
    }
  },

  async populatePlayerSelect() {
    try {
      const res     = await api.get('/Players/Active');
      const players = res.data ?? [];
      const sel     = document.getElementById('reg-player');
      sel.innerHTML =
        '<option value="">Seleccionar jugador...</option>' +
        players.map(p => `<option value="${p.id}">${p.name} ${p.lastName}</option>`).join('');
    } catch (e) {
      Toast.error('Error al cargar jugadores: ' + e.message);
    }
  },

  initForms() {

    // Carga automática al seleccionar torneo en el filtro
    document.getElementById('filter-registration-tournament')
      .addEventListener('change', () => {
        const tid = document.getElementById('filter-registration-tournament').value;
        this.loadByTournament(tid);
      });

    // Botón Cargar como respaldo
    document.getElementById('btn-load-registrations')
      .addEventListener('click', () => {
        const tid = document.getElementById('filter-registration-tournament').value;
        this.loadByTournament(tid);
      });

    // Abrir modal nueva inscripción
    document.getElementById('btn-new-registration')
      .addEventListener('click', async () => {
        await this.populateTournamentSelects();
        await this.populatePlayerSelect();
        document.getElementById('form-registration').reset();

        // Pre-seleccionar el torneo activo del filtro si hay uno
        const tid = document.getElementById('filter-registration-tournament').value;
        if (tid) document.getElementById('reg-tournament').value = tid;

        Modal.open('modal-registration');
      });

    // Submit nueva inscripción
    // NOTA: el backend solo permite inscribir en torneos con status Programmed (0)
    // El status inicial siempre será Pending — se cambia después con "Actualizar"
    document.getElementById('form-registration').addEventListener('submit', async e => {
      e.preventDefault();

      const tournamentId = parseInt(document.getElementById('reg-tournament').value);
      const playerId     = parseInt(document.getElementById('reg-player').value);
      const dorsalRaw    = document.getElementById('reg-dorsal').value;
      const paymentFee   = document.getElementById('reg-payment').checked;

      if (!tournamentId || !playerId) {
        Toast.warning('Selecciona torneo y jugador.');
        return;
      }

      const body = {
        tournamentId,
        playerId,
        dorsalNumber: dorsalRaw ? parseInt(dorsalRaw) : null,
        paymentFee,
      };

      try {
        await api.post('/TournamentRegistration', body);
        Toast.success('Jugador inscrito correctamente. Estado inicial: Pendiente.');
        Modal.close('modal-registration');

        // Recargar la tabla si hay un torneo seleccionado en el filtro
        const tid = document.getElementById('filter-registration-tournament').value;
        if (tid) this.loadByTournament(tid);
      } catch (e) {
        // Mostrar el mensaje del backend (ej: torneo no está en Programmed)
        Toast.error('Error al inscribir: ' + e.message);
      }
    });

    // Submit actualizar estado de inscripción
    // El select envía 0,1,2,3 que coinciden con el enum RegistrationStatus del backend
    document.getElementById('form-registration-status').addEventListener('submit', async e => {
      e.preventDefault();

      const id      = document.getElementById('reg-status-id').value;
      const status  = parseInt(document.getElementById('reg-new-status').value);
      const payment = document.getElementById('reg-status-payment').checked;

      if (!id) { Toast.warning('ID de inscripción no encontrado.'); return; }

      try {
        await api.patch(`/TournamentRegistration/${id}/Status`, {
          status,
          paymentFee: payment,
        });
        Toast.success('Inscripción actualizada correctamente.');
        Modal.close('modal-registration-status');

        const tid = document.getElementById('filter-registration-tournament').value;
        if (tid) this.loadByTournament(tid);
      } catch (e) {
        Toast.error('Error al actualizar: ' + e.message);
      }
    });
  },
};

  


/* ============================================================
   RONDAS  — FIX #3: carga automática
             FIX #4: número y fecha de inicio automáticos
                     fecha de cierre automática al cambiar estado
   ============================================================ */
const Rounds = {
  currentTournamentId: null,
 
  async init() {
    await this.populateTournamentSelect();
  },
 
  async populateTournamentSelect() {
    try {
      const res         = await api.get('/Tournament/Active');
      const tournaments = res.data ?? [];
      ['filter-round-tournament', 'round-tournament'].forEach(id => {
        const sel = document.getElementById(id);
        if (!sel) return;
        const cur = sel.value;
        sel.innerHTML = '<option value="">Seleccionar torneo...</option>' +
          tournaments.map(t => `<option value="${t.id}">${t.name}</option>`).join('');
        sel.value = cur;
      });
    } catch (e) {
      Toast.error('Error al cargar torneos: ' + e.message);
    }
  },
 
  async loadByTournament(tournamentId) {
    this.currentTournamentId = tournamentId;
    const tbody = document.getElementById('rounds-body');
    if (!tournamentId) { tbody.innerHTML = Render.empty(9, 'Selecciona un torneo.'); return; }
    tbody.innerHTML = Render.empty(9, 'Cargando...');
    try {
      const [roundsRes, currentRes] = await Promise.allSettled([
        api.get(`/Rounds/Tournament/${tournamentId}`),
        api.get(`/Rounds/Tournament/${tournamentId}/Current`),
      ]);
      const rounds  = roundsRes.value?.data ?? [];
      const current = currentRes.value?.data ?? null;
 
      const banner = document.getElementById('current-round-banner');
      if (current) {
        document.getElementById('current-round-label').textContent    = `Ronda ${current.roundNumber}`;
        document.getElementById('current-round-tables').textContent   = `${current.totalTable} mesas`;
        document.getElementById('current-round-progress').textContent = `${current.completeTable} completadas`;
        banner.hidden = false;
      } else {
        banner.hidden = true;
      }
 
      if (!rounds.length) { tbody.innerHTML = Render.empty(9, 'No hay rondas para este torneo.'); return; }
 
      tbody.innerHTML = rounds.map(r => `
        <tr>
          <td>${r.id}</td>
          <td><strong>Ronda ${r.roundNumber}</strong></td>
          <td>${Render.badge(r.status)}</td>
          <td>${Render.date(r.startDate)}</td>
          <td>${Render.date(r.endDate)}</td>
          <td>${r.totalTable}</td>
          <td>${r.completeTable}</td>
          <td>${Render.duration(r.duration)}</td>
          <td>
            ${Render.actionBtns([
              { action: 'change-round-status', id: r.id, label: 'Estado',   icon: '🔄', style: 'secondary' },
              { action: 'delete-round',        id: r.id, label: 'Eliminar', icon: '🗑️', style: 'danger' },
            ])}
          </td>
        </tr>
      `).join('');
      this.bindTableActions();
    } catch (e) {
      tbody.innerHTML = Render.empty(9, 'Error al cargar rondas.');
      Toast.error(e.message);
    }
  },
 
  bindTableActions() {
    document.querySelectorAll('#rounds-body [data-action]').forEach(btn => {
      btn.addEventListener('click', () => {
        const id = parseInt(btn.dataset.id);
        switch (btn.dataset.action) {
          case 'change-round-status': this.openStatus(id); break;
          case 'delete-round':        this.delete(id);     break;
        }
      });
    });
  },
 
  openStatus(id) {
    parseInt(document.getElementById('round-status-id')).value  = id;
    document.getElementById('round-new-status').value = '';
    /* FIX #4: fecha de cierre se setea automáticamente con la hora actual */
    document.getElementById('round-end-date').value   = toDatetimeLocal(new Date().toISOString());
    Modal.open('modal-round-status');
  },
 
  async delete(id) {
    const ok = await Confirm.show('¿Eliminar esta ronda? Solo se pueden eliminar rondas en estado Pendiente.');
    if (!ok) return;
    try {
      await api.delete(`/Rounds/${id}`);
      Toast.success('Ronda eliminada.');
      this.loadByTournament(this.currentTournamentId);
    } catch (e) {
      Toast.error(e.message);
    }
  },
 
  /* FIX #4: calcular próximo número de ronda automáticamente */
  async getNextRoundNumber(tournamentId) {
    try {
      const res    = await api.get(`/Rounds/Tournament/${tournamentId}`);
      const rounds = res.data ?? [];
      if (!rounds.length) return 1;
      return Math.max(...rounds.map(r => r.roundNumber)) + 1;
    } catch {
      return 1;
    }
  },
 
  initForms() {
    /* FIX #3: carga automática al cambiar el selector de torneo */
    document.getElementById('filter-round-tournament').addEventListener('change', () => {
      const tid = document.getElementById('filter-round-tournament').value;
      this.loadByTournament(tid);
    });
 
    /* Mantener botón Cargar como respaldo */
    document.getElementById('btn-load-rounds').addEventListener('click', () => {
      const tid = document.getElementById('filter-round-tournament').value;
      this.loadByTournament(tid);
    });
 
    document.getElementById('btn-new-round').addEventListener('click', async () => {
      await this.populateTournamentSelect();
      document.getElementById('form-round').reset();
 
      /* FIX #4: pre-llenar torneo seleccionado y calcular siguiente número */
      if (this.currentTournamentId) {
        document.getElementById('round-tournament').value = this.currentTournamentId;
        const nextNum = await this.getNextRoundNumber(this.currentTournamentId);
        document.getElementById('round-number').value = nextNum;
      }
 
      /* FIX #4: fecha de inicio automática con la hora actual */
      document.getElementById('round-start-date').value = toDatetimeLocal(new Date().toISOString());
 
      Modal.open('modal-round');
    });
 
    /* FIX #4: cuando se cambia el torneo en el modal, recalcular número de ronda */
    document.getElementById('round-tournament').addEventListener('change', async function () {
      if (!this.value) return;
      const nextNum = await Rounds.getNextRoundNumber(parseInt(this.value));
      document.getElementById('round-number').value = nextNum;
    });
 
    document.getElementById('form-round').addEventListener('submit', async e => {
      e.preventDefault();
      const tid = parseInt(document.getElementById('round-tournament').value);
      const num = parseInt(document.getElementById('round-number').value);
      if (!tid) { Toast.warning('Selecciona un torneo.'); return; }
      if (!num) { Toast.warning('El número de ronda es requerido.'); return; }
 
      const body = {
        tournamentId: tid,
        roundNumber:  num,
        /* FIX #4: siempre enviar fecha de inicio actual */
        startDate:    document.getElementById('round-start-date').value || new Date().toISOString(),
      };
      try {
        await api.post('/Rounds', body);
        Toast.success('Ronda creada.');
        Modal.close('modal-round');
        this.loadByTournament(body.tournamentId);
      } catch (e) {
        Toast.error(e.message);
      }
    });//.bind(this));
 
    document.getElementById('form-round-status').addEventListener('submit', async e => {
      e.preventDefault();
      const id     = document.getElementById('round-status-id').value;
      const status = parseInt(document.getElementById('round-new-status')).value;
      if (!status) { Toast.warning('Selecciona un estado.'); return; }
 
      /* FIX #4: usar la fecha del campo (ya auto-rellenada) o la actual como fallback */
      const endDate = document.getElementById('round-end-date').value || toDatetimeLocal(new Date().toISOString());
 
      try {
        await api.patch(`/Rounds/${id}/Status`, { status, endDate });
        Toast.success('Estado de ronda actualizado.');
        Modal.close('modal-round-status');
        this.loadByTournament(this.currentTournamentId);
      } catch (e) {
        Toast.error(e.message);
      }
    });
 
    document.getElementById('btn-go-current-round')?.addEventListener('click', () => {
      Nav.goTo('tables');
    });
  },
};
 
/* ============================================================
   MESAS  — FIX #3: carga automática al seleccionar ronda
   ============================================================ */
const Tables = {
  currentRoundId: null,
 
  async init() {
    await this.populateRoundSelect();
  },
 
  async populateRoundSelect() {
    try {
      const tRes        = await api.get('/Tournament/Active');
      const tournaments = tRes.data ?? [];
      const sel         = document.getElementById('filter-table-round');
      const sel2        = document.getElementById('table-round');
      let options       = '<option value="">Seleccionar ronda...</option>';
 
      for (const t of tournaments.slice(0, 5)) {
        try {
          const rRes   = await api.get(`/Rounds/Tournament/${t.id}`);
          const rounds = rRes.data ?? [];
          if (rounds.length) {
            options += `<optgroup label="${t.name}">`;
            options += rounds.map(r =>
              `<option value="${r.id}">Ronda ${r.roundNumber} (${r.status})</option>`
            ).join('');
            options += '</optgroup>';
          }
        } catch { /* skip */ }
      }
      [sel, sel2].forEach(s => { if (s) s.innerHTML = options; });
    } catch (e) {
      Toast.error('Error al cargar rondas: ' + e.message);
    }
  },
 
  async loadByRound(roundId, statusFilter = '') {
    this.currentRoundId = roundId;
    const grid = document.getElementById('tables-grid');
    if (!roundId) {
      grid.innerHTML = '<p class="tables-grid__empty">Selecciona una ronda para ver las mesas.</p>';
      return;
    }
    grid.innerHTML = '<p class="tables-grid__empty">Cargando...</p>';
    try {
      const endpoint = statusFilter === 'Pending'
        ? `/Tables/Round/${roundId}/Pending`
        : `/Tables/Round/${roundId}`;
      const res    = await api.get(endpoint);
      const tables = res.data ?? [];
      if (!tables.length) { grid.innerHTML = '<p class="tables-grid__empty">No hay mesas para esta ronda.</p>'; return; }
      grid.innerHTML = tables.map(t => `
        <div class="table-card" data-status="${t.status}" data-id="${t.id}">
          <span class="table-card__label">Mesa</span>
          <span class="table-card__number">${t.tableNumber}</span>
          ${Render.badge(t.status)}
          ${t.wonBlock ? '<span class="badge badge--Completed">🔒 Bloqueo</span>' : ''}
          <div class="table-card__actions">
            <button class="btn btn--sm btn--secondary" data-action="table-status"  data-id="${t.id}">🔄 Estado</button>
            <button class="btn btn--sm btn--secondary" data-action="table-results" data-id="${t.id}">📋 Resultados</button>
            <button class="btn btn--sm btn--danger"    data-action="delete-table"  data-id="${t.id}">🗑️</button>
          </div>
        </div>
      `).join('');
      this.bindCardActions();
    } catch (e) {
      grid.innerHTML = '<p class="tables-grid__empty">Error al cargar mesas.</p>';
      Toast.error(e.message);
    }
  },
 
  bindCardActions() {
    document.querySelectorAll('.table-card [data-action]').forEach(btn => {
      btn.addEventListener('click', e => {
        e.stopPropagation();
        const id = parseInt(btn.dataset.id);
        switch (btn.dataset.action) {
          case 'table-status':  this.openStatus(id);  break;
          case 'table-results': this.openResults(id); break;
          case 'delete-table':  this.delete(id);      break;
        }
      });
    });
  },
 
  openStatus(id) {
    document.getElementById('table-status-id').value = id;
    document.getElementById('form-table-status').reset();
    document.getElementById('table-status-id').value = id;
    Modal.open('modal-table-status');
  },
 
  async openResults(id) {
    try {
      const res   = await api.get(`/Tables/${id}/Results`);
      const table = res.data;
      const results   = table.results ?? [];
      const container = document.getElementById('table-results-body');
      if (!results.length) {
        container.innerHTML = '<p style="padding:1rem;color:var(--text-muted)">No hay resultados registrados para esta mesa.</p>';
      } else {
        container.innerHTML = `
          <div class="results-detail-grid">
            ${results.map(r => `
              <div class="result-detail-card ${r.isWinner ? 'result-detail-card--winner' : ''}">
                <div class="result-detail-card__player">
                  ${r.player?.fullName ?? `Jugador #${r.playerId}`}
                  ${r.playmate ? ` + ${r.playmate.fullName}` : ''}
                </div>
                <div class="result-detail-card__stats">
                  <span>Pts: <strong>${r.point}</strong></span>
                  <span>Acum: <strong>${r.pointsAccumulated}</strong></span>
                  <span>Elo: <strong>${r.elo >= 0 ? '+' : ''}${r.elo}</strong></span>
                </div>
                <div>${Render.badge(r.isWinner ? 'Confirmed' : 'Pending')}${r.wonBlock ? '<span class="badge badge--Completed">Bloqueo</span>' : ''}</div>
              </div>
            `).join('')}
          </div>`;
      }
      document.getElementById('modal-table-results-title').textContent = `Resultados — Mesa ${table.tableNumber}`;
      Modal.open('modal-table-results');
    } catch (e) {
      Toast.error(e.message);
    }
  },
 
  async delete(id) {
    const ok = await Confirm.show('¿Eliminar esta mesa? Solo se pueden eliminar mesas en estado Pendiente.');
    if (!ok) return;
    try {
      await api.delete(`/Tables/${id}`);
      Toast.success('Mesa eliminada.');
      this.loadByRound(this.currentRoundId);
    } catch (e) {
      Toast.error(e.message);
    }
  },
 
  initForms() {
    /* FIX #3: carga automática al seleccionar ronda */
    document.getElementById('filter-table-round').addEventListener('change', () => {
      const rid    = document.getElementById('filter-table-round').value;
      const status = parseInt(document.getElementById('filter-table-status')).value;
      this.loadByRound(rid, status);
    });
 
    document.getElementById('filter-table-status').addEventListener('change', () => {
      const rid    = document.getElementById('filter-table-round').value;
      const status = parseInt(document.getElementById('filter-table-status')).value;
      if (rid) this.loadByRound(rid, status);
    });
 
    /* Mantener botón Cargar como respaldo */
    document.getElementById('btn-load-tables').addEventListener('click', () => {
      const rid    = document.getElementById('filter-table-round').value;
      const status = parseInt(document.getElementById('filter-table-status')).value;
      this.loadByRound(rid, status);
    });
 
    document.getElementById('btn-new-table').addEventListener('click', async () => {
      await this.populateRoundSelect();
      document.getElementById('form-table').reset();
      if (this.currentRoundId) {
        document.getElementById('table-round').value = this.currentRoundId;
      }
      Modal.open('modal-table');
    });
 
    document.getElementById('form-table').addEventListener('submit', async e => {
      e.preventDefault();
      const body = {
        roundId:     parseInt(document.getElementById('table-round').value),
        tableNumber: parseInt(document.getElementById('table-number').value),
      };
      if (!body.roundId || !body.tableNumber) { Toast.warning('Completa los campos requeridos.'); return; }
      try {
        await api.post('/Tables', body);
        Toast.success('Mesa creada.');
        Modal.close('modal-table');
        this.loadByRound(body.roundId);
      } catch (e) {
        Toast.error(e.message);
      }
    });
 
    document.getElementById('form-table-status').addEventListener('submit', async e => {
      e.preventDefault();
      const id     = document.getElementById('table-status-id').value;
      const status = parseInt(document.getElementById('table-new-status')).value;
      if (!status) { Toast.warning('Selecciona un estado.'); return; }
      const body = {
        status,
        endDate:        document.getElementById('table-end-date').value || null,
        remainingChips: parseInt(document.getElementById('table-remaining-chips').value) || null,
        wonBlock:       document.getElementById('table-won-block').checked,
      };
      try {
        await api.patch(`/Tables/${id}/Status`, body);
        Toast.success('Estado de mesa actualizado.');
        Modal.close('modal-table-status');
        this.loadByRound(this.currentRoundId);
      } catch (e) {
        Toast.error(e.message);
      }
    });
  },
};
 
/* ============================================================
   RESULTADOS
   ============================================================ */
const Results = {
  async init() { },
 
  async loadByTable(tableId) {
    const tbody = document.getElementById('results-body');
    try {
      const res = await api.get(`/Results/Table/${tableId}`);
      this.renderTable(res.data ?? []);
    } catch (e) {
      tbody.innerHTML = Render.empty(11, 'Error al cargar resultados.');
      Toast.error(e.message);
    }
  },
 
  async loadByPlayer(playerId) {
    try {
      const res = await api.get(`/Results/Player/${playerId}`);
      this.renderTable(res.data ?? []);
    } catch (e) {
      document.getElementById('results-body').innerHTML = Render.empty(11, 'Error al cargar resultados.');
      Toast.error(e.message);
    }
  },
 
  renderTable(list) {
    const tbody = document.getElementById('results-body');
    if (!list.length) { tbody.innerHTML = Render.empty(11, 'No se encontraron resultados.'); return; }
    tbody.innerHTML = list.map(r => `
      <tr>
        <td>${r.id}</td>
        <td>${r.tableId}</td>
        <td class="name-cell">${r.player?.fullName ?? `#${r.playerId}`}</td>
        <td>${r.playmate?.fullName ?? '—'}</td>
        <td><strong>${r.point}</strong></td>
        <td>${r.pointsAccumulated}</td>
        <td>${r.position === 1 ? '🥇 1°' : '2°'}</td>
        <td>${r.isWinner ? '<span class="bool-yes">Sí</span>' : '<span class="bool-no">No</span>'}</td>
        <td>${r.wonBlock ? '<span class="bool-yes">Sí</span>' : '<span class="bool-no">No</span>'}</td>
        <td><span class="${r.elo >= 0 ? 'bool-yes' : 'bool-no'}">${r.elo >= 0 ? '+' : ''}${r.elo}</span></td>
        <td>
          ${Render.actionBtns([
            { action: 'edit-result',   id: r.id, label: 'Editar',   icon: '✏️', style: 'secondary' },
            { action: 'delete-result', id: r.id, label: 'Eliminar', icon: '🗑️', style: 'danger' },
          ])}
        </td>
      </tr>
    `).join('');
    this.bindTableActions();
  },
 
  bindTableActions() {
    document.querySelectorAll('#results-body [data-action]').forEach(btn => {
      btn.addEventListener('click', () => {
        const id = parseInt(btn.dataset.id);
        switch (btn.dataset.action) {
          case 'edit-result':   this.openEdit(id); break;
          case 'delete-result': this.delete(id);   break;
        }
      });
    });
  },
 
  async openEdit(id) {
    try {
      const res = await api.get(`/Results/${id}`);
      const r   = res.data;
      document.getElementById('result-edit-id').value           = r.id;
      document.getElementById('result-edit-point').value        = r.point;
      document.getElementById('result-edit-points-accum').value = r.pointsAccumulated;
      document.getElementById('result-edit-elo').value          = r.elo;
      document.getElementById('result-edit-position').value     = r.position;
      document.getElementById('result-edit-chips').value        = r.remainingChips ?? '';
      document.getElementById('result-edit-is-winner').checked  = r.isWinner;
      document.getElementById('result-edit-won-block').checked  = r.wonBlock;
      Modal.open('modal-result-edit');
    } catch (e) {
      Toast.error(e.message);
    }
  },
 
  async delete(id) {
    const ok = await Confirm.show('¿Eliminar este resultado? Esta acción no se puede deshacer.');
    if (!ok) return;
    try {
      await api.delete(`/Results/${id}`);
      Toast.success('Resultado eliminado.');
      const tableId  = document.getElementById('filter-result-table').value;
      const playerId = document.getElementById('filter-result-player').value;
      if (tableId)       this.loadByTable(tableId);
      else if (playerId) this.loadByPlayer(playerId);
    } catch (e) {
      Toast.error(e.message);
    }
  },
 
  async populateSelects() {
    try {
      const res     = await api.get('/Players/Active');
      const players = res.data ?? [];
      const playerOpts = '<option value="">Seleccionar jugador...</option>' +
        players.map(p => `<option value="${p.id}">${p.name} ${p.lastName}</option>`).join('');
      document.getElementById('result-player').innerHTML   = playerOpts;
      document.getElementById('result-playmate').innerHTML =
        '<option value="">Sin compañero (Singles)</option>' +
        players.map(p => `<option value="${p.id}">${p.name} ${p.lastName}</option>`).join('');
    } catch (e) {
      Toast.error('Error al cargar selects: ' + e.message);
    }
  },
 
  async populateTableSelect() {
    try {
      const tRes        = await api.get('/Tournament/Active');
      const tournaments = tRes.data ?? [];
      let options       = '<option value="">Seleccionar mesa...</option>';
      for (const t of tournaments.slice(0, 3)) {
        try {
          const rRes = await api.get(`/Rounds/Tournament/${t.id}`);
          for (const round of (rRes.data ?? []).slice(0, 5)) {
            try {
              const mRes  = await api.get(`/Tables/Round/${round.id}`);
              const mesas = mRes.data ?? [];
              if (mesas.length) {
                options += `<optgroup label="${t.name} — Ronda ${round.roundNumber}">`;
                options += mesas.map(m =>
                  `<option value="${m.id}">Mesa ${m.tableNumber} (${m.status})</option>`
                ).join('');
                options += '</optgroup>';
              }
            } catch { /* skip */ }
          }
        } catch { /* skip */ }
      }
      document.getElementById('result-table').innerHTML = options;
    } catch (e) {
      Toast.error('Error al cargar mesas: ' + e.message);
    }
  },
 
  initForms() {
    document.getElementById('btn-filter-results').addEventListener('click', () => {
      const tableId  = document.getElementById('filter-result-table').value;
      const playerId = document.getElementById('filter-result-player').value;
      if (tableId)       this.loadByTable(parseInt(tableId));
      else if (playerId) this.loadByPlayer(parseInt(playerId));
      else Toast.warning('Ingresa un ID de mesa o jugador para filtrar.');
    });
 
    document.getElementById('btn-new-result').addEventListener('click', async () => {
      await this.populateSelects();
      await this.populateTableSelect();
      document.getElementById('form-result').reset();
      document.getElementById('result-id').value = '';
      Modal.open('modal-result');
    });
 
    document.getElementById('form-result').addEventListener('submit', async e => {
      e.preventDefault();
      const body = {
        tableId:           parseInt(document.getElementById('result-table').value),
        playerId:          parseInt(document.getElementById('result-player').value),
        playmateId:        parseInt(document.getElementById('result-playmate').value) || null,
        point:             parseInt(document.getElementById('result-point').value) || 0,
        pointsAccumulated: parseInt(document.getElementById('result-points-accum').value) || 0,
        position:          parseInt(document.getElementById('result-position').value) || 2,
        elo:               parseInt(document.getElementById('result-elo').value) || 0,
        remainingChips:    parseInt(document.getElementById('result-chips').value) || null,
        isWinner:          document.getElementById('result-is-winner').checked,
        wonBlock:          document.getElementById('result-won-block').checked,
      };
      if (!body.tableId || !body.playerId) { Toast.warning('Selecciona mesa y jugador.'); return; }
      try {
        await api.post('/Results', body);
        Toast.success('Resultado registrado.');
        Modal.close('modal-result');
        const tableId = document.getElementById('filter-result-table').value;
        if (tableId) this.loadByTable(parseInt(tableId));
      } catch (e) {
        Toast.error(e.message);
      }
    });
 
    document.getElementById('form-result-edit').addEventListener('submit', async e => {
      e.preventDefault();
      const id   = document.getElementById('result-edit-id').value;
      const body = {
        point:             parseInt(document.getElementById('result-edit-point').value),
        pointsAccumulated: parseInt(document.getElementById('result-edit-points-accum').value),
        elo:               parseInt(document.getElementById('result-edit-elo').value),
        position:          parseInt(document.getElementById('result-edit-position').value),
        remainingChips:    parseInt(document.getElementById('result-edit-chips').value) || null,
        isWinner:          document.getElementById('result-edit-is-winner').checked,
        wonBlock:          document.getElementById('result-edit-won-block').checked,
      };
      try {
        await api.put(`/Results/${id}`, body);
        Toast.success('Resultado actualizado.');
        Modal.close('modal-result-edit');
        const tableId = document.getElementById('filter-result-table').value;
        if (tableId) this.loadByTable(parseInt(tableId));
      } catch (e) {
        Toast.error(e.message);
      }
    });
  },
};
 
/* ============================================================
   CLASIFICACIÓN  — FIX #3: carga automática
   ============================================================ */
const Classification = {
  async init() {
    await this.populateTournamentSelect();
    document.getElementById('filter-class-type').addEventListener('change', function () {
      document.getElementById('filter-topn-group').hidden = this.value !== 'top';
    });
  },
 
  async populateTournamentSelect() {
    try {
      const res         = await api.get('/Tournament/Active');
      const tournaments = res.data ?? [];
      ['filter-class-tournament', 'class-tournament'].forEach(id => {
        const sel = document.getElementById(id);
        if (!sel) return;
        const cur = sel.value;
        sel.innerHTML = '<option value="">Seleccionar torneo...</option>' +
          tournaments.map(t => `<option value="${t.id}">${t.name}</option>`).join('');
        sel.value = cur;
      });
    } catch (e) {
      Toast.error('Error al cargar torneos: ' + e.message);
    }
  },
 
  async load(tournamentId, type = 'all', n = 10) {
    const tbody  = document.getElementById('classification-body');
    const podium = document.getElementById('podium');
    if (!tournamentId) {
      tbody.innerHTML = Render.empty(13, 'Selecciona un torneo.');
      podium.hidden = true;
      return;
    }
    tbody.innerHTML = Render.empty(13, 'Cargando...');
    try {
      let res;
      if (type === 'final')      res = await api.get(`/Classification/Tournament/${tournamentId}/Final`);
      else if (type === 'top')   res = await api.get(`/Classification/Tournament/${tournamentId}/Top/${n}`);
      else                       res = await api.get(`/Classification/Tournament/${tournamentId}`);
 
      const list = res.data ?? [];
      if (!list.length) {
        tbody.innerHTML = Render.empty(13, 'No hay clasificación disponible para este torneo.');
        podium.hidden = true;
        return;
      }
      this.renderPodium(list);
      this.renderTable(list);
    } catch (e) {
      tbody.innerHTML = Render.empty(13, 'Error al cargar clasificación.');
      Toast.error(e.message);
    }
  },
 
  renderPodium(list) {
    const podium = document.getElementById('podium');
    const top3   = list.slice(0, 3);
    const fillPlace = (elId, player) => {
      if (!player) return;
      const el = document.getElementById(elId);
      el.querySelector('.podium__avatar').textContent = Render.initials(player.player?.fullName ?? '?');
      el.querySelector('.podium__name').textContent   = player.player?.fullName ?? `Jugador #${player.playerId}`;
    };
    fillPlace('podium-1st', top3[0]);
    fillPlace('podium-2nd', top3[1]);
    fillPlace('podium-3rd', top3[2]);
    podium.hidden = false;
  },
 
  renderTable(list) {
    const tbody = document.getElementById('classification-body');
    tbody.innerHTML = list.map((c, i) => `
      <tr>
        <td>${Render.pos(c.position || i + 1)}</td>
        <td class="name-cell">${c.player?.fullName ?? `Jugador #${c.playerId}`}</td>
        <td><strong>${c.winGame}</strong></td>
        <td>${c.lostGame}</td>
        <td>${c.totalGame}</td>
        <td>${c.winRate?.toFixed(1) ?? 0}%</td>
        <td>${c.favorPoints}</td>
        <td>${c.pointsAgainst}</td>
        <td class="${c.pointsDifferent >= 0 ? 'bool-yes' : 'bool-no'}">${c.pointsDifferent >= 0 ? '+' : ''}${c.pointsDifferent}</td>
        <td>${c.totalPercentage}%</td>
        <td>${c.lastRoundCalculated}</td>
        <td>${Render.bool(c.isFinal)}</td>
        <td>
          ${Render.actionBtns([
            { action: 'delete-classification', id: c.id, label: 'Eliminar', icon: '🗑️', style: 'danger' },
          ])}
        </td>
      </tr>
    `).join('');
    this.bindTableActions();
  },
 
  bindTableActions() {
    document.querySelectorAll('#classification-body [data-action]').forEach(btn => {
      btn.addEventListener('click', () => {
        const id = parseInt(btn.dataset.id);
        if (btn.dataset.action === 'delete-classification') this.delete(id);
      });
    });
  },
 
  async delete(id) {
    const ok = await Confirm.show('¿Eliminar este registro de clasificación?');
    if (!ok) return;
    try {
      await api.delete(`/Classification/${id}`);
      Toast.success('Clasificación eliminada.');
      const tid  = document.getElementById('filter-class-tournament').value;
      const type = document.getElementById('filter-class-type').value;
      const n    = parseInt(document.getElementById('filter-class-topn').value) || 10;
      this.load(tid, type, n);
    } catch (e) {
      Toast.error(e.message);
    }
  },
 
  async populatePlayerSelect() {
    try {
      const res     = await api.get('/Players/Active');
      const players = res.data ?? [];
      document.getElementById('class-player').innerHTML =
        '<option value="">Seleccionar...</option>' +
        players.map(p => `<option value="${p.id}">${p.name} ${p.lastName}</option>`).join('');
    } catch (e) {
      Toast.error('Error al cargar jugadores: ' + e.message);
    }
  },
 
  initForms() {
    /* FIX #3: carga automática al seleccionar torneo */
    document.getElementById('filter-class-tournament').addEventListener('change', () => {
      const tid  = document.getElementById('filter-class-tournament').value;
      const type = document.getElementById('filter-class-type').value;
      const n    = parseInt(document.getElementById('filter-class-topn').value) || 10;
      if (tid) this.load(tid, type, n);
    });
 
    document.getElementById('filter-class-type').addEventListener('change', () => {
      const tid  = document.getElementById('filter-class-tournament').value;
      const type = document.getElementById('filter-class-type').value;
      const n    = parseInt(document.getElementById('filter-class-topn').value) || 10;
      if (tid) this.load(tid, type, n);
    });
 
    /* Mantener botón Cargar como respaldo */
    document.getElementById('btn-load-classification').addEventListener('click', () => {
      const tid  = document.getElementById('filter-class-tournament').value;
      const type = document.getElementById('filter-class-type').value;
      const n    = parseInt(document.getElementById('filter-class-topn').value) || 10;
      this.load(tid, type, n);
    });
 
    document.getElementById('btn-upsert-classification').addEventListener('click', async () => {
      await this.populateTournamentSelect();
      const tid = document.getElementById('filter-class-tournament').value;
      if (tid) {
        document.getElementById('class-tournament').value = tid;
        await this.populatePlayerSelect(tid);
      }
      document.getElementById('form-classification').reset();
      Modal.open('modal-classification');
    });
 
    document.getElementById('class-tournament').addEventListener('change', async function () {
      await Classification.populatePlayerSelect(this.value);
    });
 
    document.getElementById('form-classification').addEventListener('submit', async e => {
      e.preventDefault();
      const body = {
        tournamentId:        parseInt(document.getElementById('class-tournament').value),
        playerId:            parseInt(document.getElementById('class-player').value),
        position:            parseInt(document.getElementById('class-position').value) || 1,
        winGame:             parseInt(document.getElementById('class-win-game').value) || 0,
        lostGame:            parseInt(document.getElementById('class-lost-game').value) || 0,
        favorPoints:         parseInt(document.getElementById('class-favor-points').value) || 0,
        pointsAgainst:       parseInt(document.getElementById('class-against-points').value) || 0,
        totalPercentage:     parseInt(document.getElementById('class-percentage').value) || 0,
        lastRoundCalculated: parseInt(document.getElementById('class-last-round').value) || 0,
        isFinal:             document.getElementById('class-is-final').checked,
      };
      if (!body.tournamentId || !body.playerId) { Toast.warning('Selecciona torneo y jugador.'); return; }
      try {
        await api.put('/Classification', body);
        Toast.success('Clasificación actualizada.');
        Modal.close('modal-classification');
        Classification.load(body.tournamentId);
      } catch (e) {
        Toast.error(e.message);
      }
    });
  },
};
 
/* ============================================================
   HELPERS
   ============================================================ */
function toDatetimeLocal(iso) {
  if (!iso) return '';
  const d = new Date(iso);
  if (isNaN(d.getTime())) return '';
  /* Ajustar a la zona local del navegador */
  const offset = d.getTimezoneOffset() * 60000;
  return new Date(d.getTime() - offset).toISOString().slice(0, 16);
}
 
/* ============================================================
   INICIALIZACIÓN GLOBAL
   ============================================================ */
document.addEventListener('DOMContentLoaded', () => {
  Toast.init();
  Modal.init();
  Confirm.init();
  Sidebar.init();
  Nav.init();
 
  Tournaments.initForms();
  Players.initForms();
  Registrations.initForms();
  Rounds.initForms();
  Tables.initForms();
  Results.initForms();
  Classification.initForms();
 
  Nav.goTo('dashboard');
});