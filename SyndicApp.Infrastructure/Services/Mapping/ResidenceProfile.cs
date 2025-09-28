using AutoMapper;
using SyndicApp.Application.DTOs.Residences;
using SyndicApp.Domain.Entities.Residences;

namespace SyndicApp.Infrastructure.Services.Mapping
{
    public class ResidenceProfile : Profile
    {
        public ResidenceProfile()
        {
            // Residence
            CreateMap<Residence, ResidenceDto>();
            CreateMap<CreateResidenceDto, Residence>();
            // UpdateResidenceDto -> manuelle dans service (Trim etc.)

            // Lot
            CreateMap<Lot, LotDto>();
            CreateMap<CreateLotDto, Lot>();
            // UpdateLotDto -> manuelle (shadow BatimentId)

            // Batiment
            CreateMap<Batiment, BatimentDto>();
            CreateMap<CreateBatimentDto, Batiment>();

            // AffectationLot
            CreateMap<AffectationLot, AffectationLotDto>();
            CreateMap<CreateAffectationLotDto, AffectationLot>();
            // ClotureAffectationLotDto utilisée juste pour DateFin (handled in service)

            // LocataireTemporaire
            CreateMap<LocataireTemporaire, LocataireTemporaireDto>();
            CreateMap<CreateLocataireTemporaireDto, LocataireTemporaire>();
            CreateMap<UpdateLocataireTemporaireDto, LocataireTemporaire>();
        }
    }
}
