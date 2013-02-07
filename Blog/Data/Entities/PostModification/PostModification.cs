using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Blog.Data
{
    public class PostModification
    {
        public int Id { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }

        public DateTime Timestamp { get; set; }

        public bool TitleModified { get; set; }
        public string NewTitle { get; set; }
        public bool DescriptionModified { get; set; }
        public string NewDescription { get; set; }
        public bool BodyModified { get; set; }
        public string NewBody { get; set; }
        public bool StatusModified { get; set; }
        public PostStatus? NewStatus { get; set; }
        public bool CannonicalModified { get; set; }
        public string NewCannonical { get; set; }

        public static PostModification GetUnmodifiedPostModification()
        {
            return new PostModification
            { 
                BodyModified = false, 
                CannonicalModified = false, 
                DescriptionModified = false, 
                StatusModified = false, 
                TitleModified = false,
                Timestamp = DateTime.Now
            };
        }
    }
}
