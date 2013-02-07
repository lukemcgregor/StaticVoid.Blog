using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Blog.Data
{
    public static class PostModificationExtensions
    {
        public static bool HasModifications(this PostModification modification)
        {
            return modification.BodyModified || 
                modification.CannonicalModified || 
                modification.DescriptionModified || 
                modification.StatusModified || 
                modification.TitleModified;
        }
    }
}
