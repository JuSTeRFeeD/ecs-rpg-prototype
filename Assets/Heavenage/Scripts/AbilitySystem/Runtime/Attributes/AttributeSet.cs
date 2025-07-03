using System.Collections.Generic;
using System.Linq;

namespace Heavenage.Scripts.AbilitySystem.Runtime.Attributes
{
    public class AttributeSet
    {
        private List<Attribute> _attributes;

        public AttributeSet(IReadOnlyList<AttributeSetup> attributesSetup)
        {
            _attributes = new List<Attribute>(attributesSetup.Count);
            for (var i = 0; i < _attributes.Count; i++)
            {
                _attributes[i] = new Attribute(attributesSetup[i]);
            }
        }
        
        public Attribute GetAttributeByDefinition(AttributeDefinition definition)
        {
            return _attributes.FirstOrDefault(attribute => attribute.Definition == definition);
        }
    }
}