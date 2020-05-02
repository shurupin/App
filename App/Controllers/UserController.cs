namespace App.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using App.DataLayer.DbContext;
    using App.Models.Core;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ReactAdminController<User>
    {
        public UserController(DefaultDbContext context) : base(context)
        {
            this._table = context.Users;
        }
    }
}
