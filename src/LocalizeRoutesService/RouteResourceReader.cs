﻿using System;
using System.Resources;
using System.Collections;

namespace IHomer.Services.LocalizeRoutes
{
    /// <summary>
    /// Required simple IResourceReader implementation. A ResourceReader
    /// is little more than an Enumeration interface that allows 
    /// parsing through the Resources in a Resource Set which
    /// is passed in the constructor.
    /// </summary>
    public class RouteResourceReader : IResourceReader
    {
        private readonly IDictionary _resources;

        public RouteResourceReader(IDictionary resources)
        {
            _resources = resources;
        }
        IDictionaryEnumerator IResourceReader.GetEnumerator()
        {
            return _resources.GetEnumerator();
        }
        void IResourceReader.Close()
        {
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _resources.GetEnumerator();
        }
        void IDisposable.Dispose()
        {
        }
    }
}
