﻿using System;
using System.Threading.Tasks;

namespace AppForeach.Framework
{
    public class HandlerExecutor : IHandlerExecutor
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IHandlerMap handlerMap;

        public HandlerExecutor(IServiceLocator serviceLocator, IHandlerMap handlerMap)
        {
            this.serviceLocator = serviceLocator;
            this.handlerMap = handlerMap;
        }

        public async Task<object> Execute(object operationInput)
        {
            Type operationType = operationInput.GetType();

            var handlerMethod = handlerMap.GetHandlerMethod(operationType);

            object handler = serviceLocator.GetService(handlerMethod.DeclaringType);

            Task task = (Task)handlerMethod.Invoke(handler, new object[] { operationInput });

            await task;

            var property = task.GetType().GetProperty("Result");
            return property.GetValue(task);
        }
    }
}
