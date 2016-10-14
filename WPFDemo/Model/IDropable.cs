using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.Model
{
    public interface IDropable
    {
        void Add(object obj);
        void Remove(object obj);
    }
}
