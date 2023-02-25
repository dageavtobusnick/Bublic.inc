using System;

public class ModuleUIItem : UIItemBase
{
    protected override void OnEquip(ItemScript item)
    {
        if(item!=null&&item.TryGetComponent<ModuleScript>(out var module))
        {
            module.Activate(true);
        }
    }

    protected override void OnDeequip(ItemScript item)
    {
        if (item != null&&item.TryGetComponent<ModuleScript>(out var module))
        {
            module.Activate(false);
        }
    }
}
