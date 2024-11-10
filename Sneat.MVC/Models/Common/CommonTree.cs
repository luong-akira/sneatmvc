﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Sneat.MVC.Models.Common
{
    public static class CommonTree
    {
        public static IEnumerable<TreeItem<T>> GenerateTree<T, K>(
           this IEnumerable<T> collection,
           Func<T, K> id_selector,
           Func<T, K> parent_id_selector,
           K root_id = default(K)
       )
        {
            foreach (var c in collection.Where(c => EqualityComparer<K>.Default.Equals(parent_id_selector(c), root_id)))
            {
                yield return new TreeItem<T>
                {
                    Item = c,
                    Children = collection.GenerateTree(id_selector, parent_id_selector, id_selector(c))
                };
            }
        }
    }
}