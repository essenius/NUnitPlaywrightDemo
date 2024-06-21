// Copyright 2024 Rik Essenius
//
//   Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file 
//   except in compliance with the License. You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software distributed under the License 
//   is distributed on an "AS IS" BASIS WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and limitations under the License.

namespace NUnitPlaywrightDemo;

[Parallelizable(ParallelScope.Self), TestFixture]

public class AttributeTest : PageTest
{
    [Test]
    public async Task AttributeOfElementTest()
    {
        // Open the test page
        await Page.GotoAsync(AppConfig.Get("TestPage"));

        // Get the iframe element and check its src attribute
        var iframeElement = await Page.QuerySelectorAsync("#iframe1");
        Assert.That(iframeElement, Is.Not.Null);
        var srcAttribute = await iframeElement.GetAttributeAsync("src");
        Assert.That(srcAttribute, Is.EqualTo("/iframe1.html"));

        // Get the class attribute of the status element
        var statusElement = await Page.QuerySelectorAsync("#status");
        Assert.That(statusElement, Is.Not.Null);
        var originalClass = await statusElement.GetAttributeAsync("class");

        // Change the class attribute to 'fail' and check the color is red
        await Page.EvaluateAsync("element => element.className = 'fail'", statusElement);
        var failColor = await statusElement.EvaluateAsync<string>("element => getComputedStyle(element).backgroundColor");
        Assert.That(IsRgb(failColor, 204, 0, 0), Is.True, "Color of fail class is red");

        // Change the class attribute to 'success' and check the color is green
        await Page.EvaluateAsync("element => element.className = 'success'", statusElement);
        var successColor = await statusElement.EvaluateAsync<string>("element => getComputedStyle(element).backgroundColor");
        Assert.That(IsRgb(successColor, 0, 204, 0), Is.True, "Color of success class is green");

        // Change the class attribute back to the original value
        await Page.EvaluateAsync($"element => element.className = '{originalClass}'", statusElement);
    }

    private static bool IsRgb(string cssColor, int r, int g, int b)
    {
        // Assuming cssColor is in the format "rgb(r, g, b)"
        var rgb = cssColor.Replace("rgb(", "").Replace(")", "").Split(',');
        return int.Parse(rgb[0].Trim()) == r && int.Parse(rgb[1].Trim()) == g && int.Parse(rgb[2].Trim()) == b;
    }

}

