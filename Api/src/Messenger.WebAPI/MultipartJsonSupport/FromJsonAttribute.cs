using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.MultipartJsonSupport
{
    public class FromJsonAttribute : ModelBinderAttribute
    {
        public FromJsonAttribute() : base(typeof(FormDataJsonBinder))
        {
        }
    }
}
