using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSET.Framework.Service.Infrastructure.Model
{
    public enum ResponseStatusCode
    {
        [ResponseStatusCode(200, "")]
        Code200,

        [ResponseStatusCode(400, "Input validation error")]
        Code400,

        [ResponseStatusCode(500, "Internal server error")]
        Code500,

        [ResponseStatusCode(501, "An error ocurred processing the request")]
        Code501,

        [ResponseStatusCode(502, "An error ocurred saving the request in the database")]
        Code502
    }
}