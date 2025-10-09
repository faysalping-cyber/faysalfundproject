namespace FaysalFunds.Application.DTOs.AccountOpening
{
    public class RiskProfileDropDownDTO : DropDownDTO
    {
        public List<QuestionnaireItemDTO> Body { get; set; }

    }

    public class QuestionnaireItemDTO
    {
        public string Key { get; set; }
        public string Question { get; set; }
        public List<DropDownDTO> Answers { get; set; }
    }
}
