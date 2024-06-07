using CredmargAPI.Models;
using CredmargAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CredmargAPI.Controllers
{
    [Route("api/vendor")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly CredmargRepository _repository;

        public VendorController(CredmargRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("create-vendor")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateVendor([FromBody] Vendor vendor)
        {
            _repository.Vendors.Add(vendor);
            return Ok();
        }

        [HttpGet("getall-vendors")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetVendors()
        {
            return Ok(_repository.Vendors);
        }
    }
}
