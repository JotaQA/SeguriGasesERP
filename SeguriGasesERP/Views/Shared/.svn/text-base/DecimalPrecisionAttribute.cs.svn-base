using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;

public class DecimalPrecisionAttribute : Attribute
{
    int _precision;
    private int _scale;

    public DecimalPrecisionAttribute(int precision, int scale)
    {
        _precision = precision;
        _scale = scale;
    }

    public int Precision { get { return _precision; } }
    public int Scale { get { return _scale; } }



}
/*
public class DecimalPrecisionAttributeConvention : AttributeConfigurationConvention<PropertyInfo, DecimalPropertyConfiguration, DecimalPrecisionAttribute>
{
    public override void Apply(PropertyInfo memberInfo, DecimalPropertyConfiguration configuration, DecimalPrecisionAttribute attribute)
    {  
        configuration.Precision = Convert.ToByte(attribute.Precision); 
        configuration.Scale = Convert.ToByte(attribute.Scale);
    }
}
 
*/
    