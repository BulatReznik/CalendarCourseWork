using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CalendarCourseWork.DataBase.Models;
using CalendarCourseWork.Logic;
using CalendarCourseWork.Models;

namespace CalendarCourseWork.Views.Home
{
    public class RegisterModel : PageModel
    {
        private UsersLogic _usersLogic;

        public RegisterModel(UsersLogic usersLogic)
        {
            _usersLogic = usersLogic;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public UserInputModel UserInputModel { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || UserInputModel == null)
            {
                return Page();
            }

            User user = new()
            {
                Name = UserInputModel.Name,
                Email = UserInputModel.Email,
                Password = UserInputModel.Password,
            };

            User result = await _usersLogic.CreateUserAsync(user);

            if (result == null)
            {
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
