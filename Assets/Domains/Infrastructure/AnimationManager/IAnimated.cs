using UnityEngine;

namespace Animations
{
    public interface IAnimated
    {
        // No need to do anything with it. It is used for caching to prevent GetComponent expensive call
         Animation Animation { get; set; }

         GameObject GameObject { get;}
    }
}