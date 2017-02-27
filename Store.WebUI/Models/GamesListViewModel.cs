﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Store.Domain.Entities;

namespace Store.WebUI.Models
{
    public class GamesListViewModel
    {
        public IEnumerable<Game> Games { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}