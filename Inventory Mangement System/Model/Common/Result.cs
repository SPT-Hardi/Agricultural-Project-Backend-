﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class Result
    {
        public enum ResultStatus
        {
            none,
            success,
            danger,
            warning,
            info
        }

        //public Guid? LogId { get; set; }

        [JsonProperty("Result")]
        public string StatusString
        {
            get
            {
                return Status.ToString();
            }

            set
            {
                Status = (ResultStatus)(Enum.Parse(typeof(ResultStatus),value ,true ));
            }
        }

        [JsonIgnore]
        public ResultStatus Status { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}
