using AutoMapper.SelfConfig;
using Domain;
using System;
using AutoMapper;
using Domain.Commands;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class CommitteeViewModel : IMapFrom<Committee>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        [Required]
        public Guid DepartmentId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Mandate { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<CommitteeViewModel, NewCommitteeCommand>()
                .ConstructUsing(ctx => 
                new NewCommitteeCommand(
                        Guid.NewGuid(), 
                        ((CommitteeViewModel)ctx.SourceValue).Name, 
                        ((CommitteeViewModel)ctx.SourceValue).Mandate, 
                        ((CommitteeViewModel)ctx.SourceValue).DepartmentId
                    ));

            configuration.CreateMap<CommitteeViewModel, UpdateCommitteeCommand>()
                .ConstructUsing(ctx => 
                new UpdateCommitteeCommand(
                        ((CommitteeViewModel)ctx.SourceValue).Id,
                        ((CommitteeViewModel)ctx.SourceValue).DepartmentId, 
                        ((CommitteeViewModel)ctx.SourceValue).Name, 
                        ((CommitteeViewModel)ctx.SourceValue).Mandate
                    ));
        }
    }
}