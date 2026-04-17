using AutoMapper;
using Domino.Application.DTOs.ClassificationDTOs;
using Domino.Application.DTOs.PlayerDTOs;
using Domino.Application.DTOs.ResultDTOs;
using Domino.Application.DTOs.RoundDTOs;
using Domino.Application.DTOs.TableDTOs;
using Domino.Application.DTOs.TournamentDTOs;
using Domino.Application.DTOs.TournamentRegistrationDTOs;
using Domino.Domain.Entities;

namespace Domino.Application.Maping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {

            //Player
            CreateMap<Player, PlayerDTO>()
                .ForMember(dest => dest.FullName,
                           opt => opt.MapFrom(src => $"{src.Name} {src.LastName}"));

            CreateMap<Player, PlayerSummaryDTO>()
                .ForMember(dest => dest.FullName,
                           opt => opt.MapFrom(src => $"{src.Name} {src.LastName}"));

            CreateMap<CreatePlayerDTO, Player>();
            


            CreateMap<UpdatePlayerDTO, Player>();
                



            //Tournament
            CreateMap<Tournament, TournamentDTO>();
            

            CreateMap<Tournament, TournamentDetailDTO>();
           

            CreateMap<CreateTournamentDTO, Tournament>();
            

            CreateMap<UpdateTournamentDTO, Tournament>();
            

            //TournamnetRegistration
            CreateMap<TournamentRegistration, TournamentRegistrationDTO>();
            
            CreateMap<CreateTournamentRegistrationDTO, TournamentRegistration>();
           


            //Round
            CreateMap<Round, RoundDTO>();
            
            CreateMap<Round, RoundSummaryDTO>();
            
            CreateMap<Round, RoundDetailDTO>();
            
            CreateMap<CreateRoundDTO, Round>();
            

            //Table
            CreateMap<Table, TableDTO>();
            
            CreateMap<Table, TableDetailDTO>();
                

            CreateMap<CreateTableDTO, Table>();
                

            //Result
            CreateMap<Result, ResultDTO>();

            CreateMap<CreateResultDTO, Result>();
            

            CreateMap<UpdateResultDTO, Result>();
               
            
            //Classification
            CreateMap<Classification, ClassificationDTO>();

            CreateMap<UpdateClassificationDTO, Classification>();
               
        }

    }
}
