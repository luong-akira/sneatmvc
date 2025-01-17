﻿using System.Collections.Generic;

namespace Sneat.MVC.Models.Common
{
    public class TreeItem<T>
    {
        public T Item { get; set; }
        public IEnumerable<TreeItem<T>> Children { get; set; }
    }
}