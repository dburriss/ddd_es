using System;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Domain;
using AutoMapper;
using Web.ViewModels;
using Domain.Commands;

namespace Web.Controllers
{
    public class CommitteeController : Controller
    {
        private readonly IApplicationCommandService _commandService;
        private readonly IDepartmentQueryRepository _departmentQueryRepository;

        public CommitteeController(IApplicationCommandService commandService, IDepartmentQueryRepository departmentQueryService)
        {
            this._commandService = commandService;
            this._departmentQueryRepository = departmentQueryService;
        }

        public IActionResult Details(Guid id, Guid departmentId)
        {
            var department = _departmentQueryRepository.GetById(departmentId);
            var committee = department.Committees.Single(x => x.Id == id);
            var vm = Mapper.Map<CommitteeViewModel>(committee);
            vm.DepartmentId = departmentId;
            return View(vm);
        }

        public IActionResult Create(Guid departmentId)
        {
            var vm = new CommitteeViewModel()
            {
                DepartmentId = departmentId
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(CommitteeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var command = Mapper.Map<NewCommitteeCommand>(viewModel);
                _commandService.Handle(command);
                return RedirectToAction("Details", "Department", new { id = viewModel.DepartmentId }).WithSuccess("Committee created successfully.");
            }
            else
            {
                return View(viewModel).WithError("Validation error occured.");
            }            
        }

        public IActionResult Update(Guid id, Guid departmentId)
        {
            var department = _departmentQueryRepository.GetById(departmentId);
            var committee = department.Committees.Single(x => x.Id == id);
            var vm = Mapper.Map<CommitteeViewModel>(committee);
            vm.DepartmentId = departmentId;
            return View(vm);
        }

        [HttpPost]
        public IActionResult Update(CommitteeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var command = Mapper.Map<UpdateCommitteeCommand>(viewModel);
                _commandService.Handle(command);
                return RedirectToAction("Details", new { id = viewModel.Id, departmentId = viewModel.DepartmentId }).WithSuccess("Department updated successfully.");
            }
            else
            {
                return View(viewModel).WithError("Validation error occured.");
            }
        }

        public IActionResult Delete(Guid id, Guid departmentId)
        {
            var department = _departmentQueryRepository.GetById(departmentId);
            var committee = department.Committees.Single(x => x.Id == id);
            var vm = Mapper.Map<CommitteeViewModel>(committee);
            vm.DepartmentId = departmentId;
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmDelete(Guid id, Guid departmentId)
        {
            if (ModelState.IsValid)
            {
                var command = new DeleteCommitteeCommand(id, departmentId);
                _commandService.Handle(command);
                return RedirectToAction("Details", "Department", new { id = departmentId }).WithSuccess("Committee deleted successfully.");
            }
            else
            {
                return RedirectToAction("Delete", new { id, departmentId }).WithError("An error occured.");
            }
        }
        
        public IActionResult Error()
        {
            return View();
        }
    }
}
