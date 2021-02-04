using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Incoding.Core.CQRS;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Extensions;
using Incoding.Web.CQRS.Common.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Incoding.Web.MvcContrib
{
    public class CreateMessageByType2
    {
        //public IServiceProvider Provider { get; set; }

        public Controller Controller { get; set; }

        public async Task<object> Execute()
        {
            //IModelMetadataProvider modelMetataMetadataProvider = Provider.GetRequiredService<IModelMetadataProvider>();
            
            var byPair = Type.Split(UrlDispatcher.separatorByPair.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string genericType = byPair.ElementAtOrDefault(1);

            var inst = new DefaultDispatcher().Query(new FindTypeByName()
            {
                Type = byPair[0],
            });
            var formCollection = new DefaultDispatcher().Query(new GetFormCollectionsQuery());
            var instanceType = IsGroup ? typeof(List<>).MakeGenericType(inst) : inst;
            if (instanceType.IsTypicalType() && IsModel)
            {
                string str = formCollection["incValue"];
                if (instanceType == typeof(string))
                    return str;
                if (instanceType == typeof(bool))
                    return bool.Parse(str);
                if (instanceType == typeof(DateTime))
                    return DateTime.Parse(str);
                if (instanceType == typeof(int))
                    return int.Parse(str);
                if (instanceType.IsEnum)
                    return Enum.Parse(instanceType, str);
            }
            else if (!string.IsNullOrWhiteSpace(genericType))
            {
                instanceType = instanceType.MakeGenericType(genericType.Split(UrlDispatcher.separatorByGeneric.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                    .Select(name => new DefaultDispatcher().Query(new FindTypeByName()
                    {
                        Type = name,
                    }))
                    .ToArray());
            }

            var obj = Activator.CreateInstance(instanceType);

            await Controller.TryUpdateModelAsync(obj, instanceType, "");

            /*
            var modelMetadata = modelMetataMetadataProvider.GetMetadataForType(instanceType);
            
            MvcOptions mvcOptions = Provider.GetRequiredService<IOptions<MvcOptions>>().Value;
            mvcOptions.ModelBinderProviders.Clear();
            mvcOptions.ModelBinderProviders.Add(new FormCollectionModelBinderProvider());
            mvcOptions.ModelBinderProviders.Add(new ComplexTypeModelBinderProvider());
            mvcOptions.ModelBinderProviders.Add(new SimpleTypeModelBinderProvider());
            mvcOptions.ModelBinderProviders.Add(new ArrayModelBinderProvider());
            mvcOptions.ModelBinderProviders.Add(new CollectionModelBinderProvider());
            mvcOptions.ModelBinderProviders.Add(new DictionaryModelBinderProvider());
            mvcOptions.ModelBinderProviders.Add(new FloatingPointTypeModelBinderProvider());
        
            var modelBinderFactory = new ModelBinderFactory(modelMetadata,
                new OptionsWrapper<MvcOptions>(mvcOptions)                
            );
            
            var valueProviderFactoryContext = new ValueProviderFactoryContext(ControllerContext);

            await new FormValueProviderFactory().CreateValueProviderAsync(valueProviderFactoryContext);
            await new QueryStringValueProviderFactory().CreateValueProviderAsync(valueProviderFactoryContext);
            await new RouteValueProviderFactory().CreateValueProviderAsync(valueProviderFactoryContext);
            await new JQueryFormValueProviderFactory().CreateValueProviderAsync(valueProviderFactoryContext);
            
            bool isValid = await ModelBindingHelper.TryUpdateModelAsync(obj, instanceType, "", ControllerContext,
                modelMetataMetadataProvider,
                modelBinderFactory, new CompositeValueProvider(valueProviderFactoryContext.ValueProviders), 
                Provider.GetService<IObjectModelValidator>());
                        
            return obj;
            */
            // ModelMetadataProviders.Current.GetMetadataForType(() => Activator.CreateInstance(instanceType), instanceType))().
            //return new ComplexTypeModelBinder().BindModelAsync(ControllerContext ?? new ControllerContext(), new ModelBindingContext()
            //                                                                                        {
            //                                                                                                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => Activator.CreateInstance(instanceType), instanceType),
            //                                                                                                ModelState = ModelState ?? new ModelStateDictionary(),                                                                                                            
            //                                                                                                ValueProvider = ControllerContext != null
            //                                                                                                                        ? ValueProviderFactories.Factories.GetValueProvider(ControllerContext)
            //                                                                                                                        : formCollection,
            //                                                                                        });
            return obj;
        }

        public class AsCommands
        {
            public string IncTypes { get; set; }

            public bool? IsComposite { get; set; }

            //public ModelStateDictionary ModelState { get; set; }

            public Controller Controller { get; set; }

            public IServiceProvider Provider { get; set; }

            public async Task<IMessage[]> Execute()
            {
                var splitByType = IncTypes.Split(UrlDispatcher.separatorByType.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                bool isCompositeAsArray = splitByType.Count() == 1 && IsComposite.GetValueOrDefault();
                if(isCompositeAsArray)
                    return ((IEnumerable<IMessage>) await new CreateMessageByType2()
                    {
                        Type = splitByType[0],
                        Controller = this.Controller,
                        IsGroup = true
                    }.Execute()).ToArray();

                List<IMessage> cbs = new List<IMessage>();
                foreach (var r in splitByType)
                {
                    IMessage cb = (IMessage)await new CreateMessageByType2() { Type = r, Controller = this.Controller }.Execute();
                    cbs.Add(cb);
                }
                return cbs.ToArray();
            }
        }

        #region Nested classes

        public   sealed class FindTypeByName : QueryBase<Type>
        {
            #region Static Fields

            static readonly ConcurrentDictionary<string, string> cache = new ConcurrentDictionary<string, string>();

            #endregion

            #region Fields

            public string Type { get; set; }

            #endregion

            protected override Type ExecuteResult()
            {
                string name = HttpUtility.UrlDecode(Type).Replace(" ", "+");
                var assmelbyName = cache.GetOrAdd(name, (i) =>
                {
                    var clearName = name.Contains("`") ? name.Split('`')[0] + "`1" : name;
                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        var findType = ReflectionExtensions.GetLoadableTypes(assembly).FirstOrDefault(type => type.Name == clearName || type.FullName == clearName);
                        if (findType != null)
                            return findType.AssemblyQualifiedName;
                    }

                    throw new IncMvdException("Not found any type {0}".F(name));
                });
                return System.Type.GetType(assmelbyName);
            }
        }

        #endregion

        public sealed class GetFormCollectionsQuery : QueryBase<FormCollection>
        {
            protected override FormCollection ExecuteResult()
            {
                var request = Dispatcher.Query(new GetHttpContextQuery()).Request;
                var dictionary = request.Query.ToDictionary(r => r.Key, r => r.Value);
                if (request.HasFormContentType)
                {
                    foreach (var form in request.Form)
                    {
                        dictionary.Add(form.Key, form.Value);
                    }
                }
                var formAndQuery = new FormCollection(dictionary);
                return formAndQuery;
            }
        }

        #region Properties

        public string Type { get; set; }

        public bool IsGroup { get; set; }

        public bool IsModel { get; set; }

        //public ModelStateDictionary ModelState { get; set; }
        
        #endregion
    }

    #region << Using >>

    #endregion


    //public class MessageModelBinder : IModelBinder
    //{
    //    public string Type { get; set; }

    //    public bool IsGroup { get; set; }

    //    public bool IsModel { get; set; }

    //    public ModelStateDictionary ModelState { get; set; }

    //    public ControllerContext ControllerContext { get; set; }

    //    public Task BindModelAsync(ModelBindingContext bindingContext)
    //    {
    //        object model = null;
    //        var byPair = Type.Split(UrlDispatcher.separatorByPair.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    //        string genericType = byPair.ElementAtOrDefault(1);

    //        var inst = new DefaultDispatcher().Query(new CreateMessageByType.FindTypeByName()
    //        {
    //            Type = byPair[0],
    //        });
    //        var formCollection = new DefaultDispatcher().Query(new CreateMessageByType.GetFormCollectionsQuery());
    //        var instanceType = IsGroup ? typeof(List<>).MakeGenericType(inst) : inst;
    //        if (instanceType.IsTypicalType() && IsModel)
    //        {
    //            string str = formCollection["incValue"];
    //            if (instanceType == typeof(string))
    //                model = str;
    //            if (instanceType == typeof(bool))
    //                model = bool.Parse(str);
    //            if (instanceType == typeof(DateTime))
    //                model = DateTime.Parse(str);
    //            if (instanceType == typeof(int))
    //                model = int.Parse(str);
    //            if (instanceType.IsEnum)
    //                model = Enum.Parse(instanceType, str);
    //            if (model != null)
    //            {
    //                bindingContext.Result = ModelBindingResult.Success(model);
    //                return Task.CompletedTask;
    //            }
    //        }
    //        else if (!string.IsNullOrWhiteSpace(genericType))
    //        {
    //            instanceType = instanceType.MakeGenericType(genericType.Split(UrlDispatcher.separatorByGeneric.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
    //                                                                   .Select(name => new DefaultDispatcher().Query(new CreateMessageByType.FindTypeByName()
    //                                                                   {
    //                                                                       Type = name,
    //                                                                   }))
    //                                                                   .ToArray());
    //        }

            
    //        bindingContext.Result = ModelBindingResult.Success(model);
    //        return Task.CompletedTask;
    //    }
    //}
}