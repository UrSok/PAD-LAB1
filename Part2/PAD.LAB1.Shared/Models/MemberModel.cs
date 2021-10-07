using PAD.LAB1.Shared.Utils;
using System;

namespace PAD.LAB1.Shared.Models
{
    public class MemberModel
    {
        public string Id { get; set; }
        public string HexColor { get; set; }
        public string Name { get; set; }

        public MemberModel(string name, string hexColor)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            HexColor = hexColor;
        }

        protected MemberModel()
        {
            Id = "";
            HexColor = "";
        }
        public static MemberModel GetSystemMember()
        {
            var memberModel = new MemberModel();
            memberModel.Name = Constants.System;
            return memberModel;
        }
    }
}