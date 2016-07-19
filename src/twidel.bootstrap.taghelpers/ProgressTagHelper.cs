using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace twidel.bootstrap.taghelpers
{
    [HtmlTargetElement("div", Attributes = ProgressValueAttributeName)]
    public class ProgressTagHelper : TagHelper
    {
        private const string ProgressValueAttributeName = "bs-progress-value";
        private const string ProgressMinAttributeName = "bs-progress-min";
        private const string ProgressMaxAttributeName = "bs-progress-max";

        [HtmlAttributeName(ProgressValueAttributeName)]
        public int ProgressValue { get; set; }

        [HtmlAttributeName(ProgressMinAttributeName)]
        public int ProgressMin { get; set; } = 0;

        [HtmlAttributeName(ProgressMaxAttributeName)]
        public int ProgressMax { get; set; } = 100;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ProgressMin >= ProgressMax)
            {
                throw new ArgumentException($"{ProgressMinAttributeName} must be less than {ProgressMaxAttributeName}");
            }

            if (ProgressValue > ProgressMax || ProgressValue < ProgressMin)
            {
                throw new ArgumentOutOfRangeException($"{ProgressValueAttributeName} must be within the range of {ProgressMinAttributeName} and {ProgressMaxAttributeName}");
            }

            var progressTotal = ProgressMax - ProgressMin;
            var progressPercentage = Math.Round((decimal)(ProgressValue - ProgressMin) / (decimal)progressTotal * 100, 0);

            string progressBarContent =
                $@"<progress class='progress' value='{ProgressValue}' max='{ProgressMax}'>
<div class='progress'><span class='progress-bar' style='width: {progressPercentage}%;'>{progressPercentage}%</span></div>
</progress>";

            output.Content.AppendHtml(progressBarContent);

            string classValue;
            if (output.Attributes.ContainsName("class"))
            {
                classValue = $"{output.Attributes["class"]} progress";
            }
            else
            {
                classValue = "progress";
            }

            output.Attributes.Add("class", classValue);

            base.Process(context, output);
        }
    }
}
