using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Datastores.Sample
{
    public class SampleElement : IDatastoreElement
    {
        public string ElementName { get { return "Element " + ElementId; } }
        public string ElementId { get; set; }

        /*
         * This is where the element's data should be!!
         */
    }
}