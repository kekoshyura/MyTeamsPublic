using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTeamsCore.Common;

public static class 
VerificationHelper{
    public static T
    VerifyType<T>(this object @object) {
        if (@object is not T tObject)
            throw new InvalidOperationException($"Object is expecting to be of type {typeof(T)}, but was {@object.GetType()}");

        return tObject;
    }
}
