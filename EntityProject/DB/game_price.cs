//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntityProject.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class game_price
    {
        public int game_id { get; set; }
        public int price { get; set; }
        public System.DateTime date_s { get; set; }
        public System.DateTime date_e { get; set; }
    
        public virtual games games { get; set; }
    }
}