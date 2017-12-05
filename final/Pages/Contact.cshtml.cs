using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace final.Pages
{
    public class ContactModel : PageModel
    {
        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public Contact Contact { get;set; }

        public IActionResult OnPost() {
            if (!ModelState.IsValid) {
                return Page();
            }

            Message = $"Thank you {Contact.Name} for contacting us. We will get back to your shortly";
            return RedirectToPage();
        }
    }
}
