using System;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Domain;
using AutoMapper;
using Web.ViewModels;
using Domain.Commands;

namespace Web.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IApplicationCommandService _commandService;
        private readonly IDepartmentQueryRepository _departmentQueryRepository;

        public DepartmentController(IApplicationCommandService commandService, IDepartmentQueryRepository departmentQueryService)
        {
            this._commandService = commandService;
            this._departmentQueryRepository = departmentQueryService;
        }

        public IActionResult Index()
        {
            var departments = _departmentQueryRepository.Query().Select(Mapper.Map<DepartmentViewModel>);            
            return View(departments);
        }

        public IActionResult Details(Guid id)
        {
            var department = _departmentQueryRepository.GetById(id);
            var vm = Mapper.Map<DepartmentViewModel>(department);
            return View(vm);
        }

        public IActionResult Create()
        {
            var department = new Department();
            var vm = Mapper.Map<DepartmentViewModel>(department);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(DepartmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var command = Mapper.Map<NewDepartmentCommand>(viewModel);
                _commandService.Handle(command);
                return RedirectToAction("Index").WithSuccess("Department created successfully.");
            }
            else
            {
                return View(viewModel).WithError("Validation error occured.");
            }            
        }

        public IActionResult Update(Guid id)
        {
            var department = _departmentQueryRepository.GetById(id);
            var vm = Mapper.Map<DepartmentViewModel>(department);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Update(DepartmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var command = Mapper.Map<UpdateDepartmentCommand>(viewModel);
                _commandService.Handle(command);
                return RedirectToAction("Index").WithSuccess("Department updated successfully.");
            }
            else
            {
                return View(viewModel).WithError("Validation error occured.");
            }
        }

        public IActionResult Delete(Guid id)
        {
            var department = _departmentQueryRepository.GetById(id);
            var vm = Mapper.Map<DepartmentViewModel>(department);
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmDelete(Guid id)
        {
            if (ModelState.IsValid)
            {
                var command = new DeleteDepartmentCommand(id);
                _commandService.Handle(command);
                return RedirectToAction("Index").WithSuccess("Department deleted successfully.");
            }
            else
            {
                return RedirectToAction("Delete", new { id }).WithError("An error occured.");
            }
        }
        
        public IActionResult Error()
        {
            return View();
        }
    }
}
