using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Moq;

namespace DisCore.Tests
{
    public static class MockMethodHelper
    {
        internal static Mock<MethodInfo> CreateMockMethodInfo(IEnumerable<Type> types)
        {
            var paramInfo = new List<ParameterInfo>();

            foreach (var type in types)
            {
                var param = new Mock<ParameterInfo>();
                param.Setup(p => p.Name).Returns($"GeneratedParam{Guid.NewGuid()}");
                param.Setup(p => p.ParameterType).Returns(type);
                paramInfo.Add(param.Object);
            }

            var methodInfo = new Mock<MethodInfo>();
            methodInfo.Setup(m => m.Name).Returns($"GeneratedMethod{Guid.NewGuid()}");
            methodInfo.Setup(m => m.GetParameters()).Returns(paramInfo.ToArray());

            return methodInfo;
        }
    }
}
