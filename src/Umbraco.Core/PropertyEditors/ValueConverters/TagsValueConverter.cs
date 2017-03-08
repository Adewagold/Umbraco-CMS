﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Umbraco.Core.Configuration;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;

namespace Umbraco.Core.PropertyEditors.ValueConverters
{
    [DefaultPropertyValueConverter]
    [PropertyValueType(typeof(IEnumerable<string>))]
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
    public class TagsValueConverter : PropertyValueConverterBase
    {
        private readonly IDataTypeService _dataTypeService;

        //TODO: Remove this ctor in v8 since the other one will use IoC
        public TagsValueConverter()
            : this(ApplicationContext.Current.Services.DataTypeService)
        {
        }

        public TagsValueConverter(IDataTypeService dataTypeService)
        {
            if (dataTypeService == null) throw new ArgumentNullException("dataTypeService");
            _dataTypeService = dataTypeService;
        }

        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            if (UmbracoConfig.For.UmbracoSettings().Content.EnablePropertyValueConverters)
            {
                return propertyType.PropertyEditorAlias.InvariantEquals(Constants.PropertyEditors.TagsAlias);
            }
            return false;
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            // if Json storage type deserialzie and return as string array
            if (JsonStorageType(propertyType.DataTypeId))
            {
                var jArray = JsonConvert.DeserializeObject<JArray>(source.ToString());
                return jArray.ToObject<string[]>();
            }

            // Otherwise assume CSV storage type and return as string array
            var csvTags =
                source.ToString()
                    .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();
            return csvTags;
        }

        public override object ConvertSourceToObject(PublishedPropertyType propertyType, object source, bool preview)
        {
            if (source == null)
            {
                return null;
            }
            return (string[]) source;
        }

        /// <summary>
        /// Discovers if the tags data type is storing its data in a Json format
        /// </summary>
        /// <param name="dataTypeId">
        /// The data type id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool JsonStorageType(int dataTypeId)
        {
            // ** This must be cached (U4-8862) **
            var storageType =
                _dataTypeService.GetPreValuesCollectionByDataTypeId(dataTypeId)
                    .PreValuesAsDictionary.FirstOrDefault(
                        x => string.Equals(x.Key, "storageType", StringComparison.InvariantCultureIgnoreCase)).Value;

            if (storageType.Value.InvariantEquals("Json"))
            {
                return true;
            }

            return false;
        }
    }
}
