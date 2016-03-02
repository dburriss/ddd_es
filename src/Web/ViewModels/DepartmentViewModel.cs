using AutoMapper.SelfConfig;
using System;
using Domain;
using System.Collections.Generic;
using AutoMapper;
using Domain.Commands;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class DepartmentViewModel : IMapFrom<Department>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public IEnumerable<CommitteeViewModel> Committees { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<DepartmentViewModel, NewDepartmentCommand>()
                .ConstructUsing(ctx => new NewDepartmentCommand(Guid.NewGuid(), ((DepartmentViewModel)ctx.SourceValue).Name));

            configuration.CreateMap<DepartmentViewModel, UpdateDepartmentCommand>()
                .ConstructUsing(ctx => new UpdateDepartmentCommand(((DepartmentViewModel)ctx.SourceValue).Id, ((DepartmentViewModel)ctx.SourceValue).Name));
        }
    }
}
