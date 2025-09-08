using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DazziorShop.Application.Common
{
    partial class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuceess { get; set; } = false;
        public object Result { get; set; }
        public string DisplayMessage { get; set; } = "";
        public List<APIError> Errors { get; set; } = new();
        public List<APIWarning> Warning { get; set; } = new();
        public void AddError(string errorMessage)
        {
            APIError error = new APIError(description:errorMessage);
            Errors.Add(error);
        }
        public void AddWarning(string errorMessage)
        {
            APIWarning warning = new APIWarning(description:errorMessage);
            Warning.Add(warning);
        }


    }
}
