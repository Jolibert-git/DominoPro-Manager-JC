using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Domain.Entities
{
    public class Enums
    {
        public enum TournamentStatus
        {
            Programmed,
            InCourse,
            Finalized,
            Canceled
        }

        public enum RoundStatus
        {
            Pending,
            InPlay,
            Completed
        }

        public enum GameStatus
        {
            Pending,
            InPlay,
            Completed,
            Cancelled
        }

        public enum TypeMode
        {
            Singles,
            Doubles
        }

        public enum RegistrationStatus
        {
            Pending,
            Confirmed,
            Withdrawn,
            Disqualified
        }
    }
}
