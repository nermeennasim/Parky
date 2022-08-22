using Microsoft.AspNetCore.Mvc;
using Parky.Web.Repository.IRepository;
using System.Threading.Tasks;

namespace Parky.Web.Controllers
{
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        public NationalParkController(INationalParkRepository npRepo)
        {
            _npRepo = npRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllNationalPark()
        {
            //using API path from SD class we are calling repo  methods
            return Json(new { data = await _npRepo.GetAllAsync(SD.NationalParkAPIPath) });
        }
    }
}
