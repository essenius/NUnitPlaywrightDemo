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
public class AlertTest : PageTest
{
    [SetUp]
    public async Task TestInitialize()
    {
        Page.Context.SetDefaultTimeout(20_000);
        await Page.GotoAsync(AppConfig.Get("TestPage"));
    }

    [Test]
    public async Task AlertTest1()
    {
        await Page.WaitForSelectorAsync("text=data load completed");

        // Find the alert button
        var locator = Page.Locator("#alertButton");
        var element = await locator.ElementHandleAsync();
        Assert.That(element, Is.Not.Null);

        // Get the button into view and click it
        await element.ScrollIntoViewIfNeededAsync();
        await element.ClickAsync();
        
        // should be automatically dismissed
        await Expect(locator).ToBeEnabledAsync();
        await Expect(locator).ToBeVisibleAsync();

        // Create a handler for the dialog event and click the button again.
        // This time we should dismiss it via the handler
        Page.Dialog += AlertDialogHandler;
        await element.ClickAsync();
        Page.Dialog -= AlertDialogHandler;

        // Do the same thing for a confirm dialog
        await Page.ClickAsync("#confirmButton");
        var statusLocator = Page.Locator("#status");
        var textContent = await statusLocator.TextContentAsync();
        Assert.That(textContent, Is.EqualTo("You pressed Cancel"));

        Page.Dialog += ConfirmDialogHandler;
        await Page.ClickAsync("#confirmButton");
        Page.Dialog -= ConfirmDialogHandler;
        textContent = await statusLocator.TextContentAsync();
        Assert.That(textContent, Is.EqualTo("You pressed OK"));

        // And for a prompt dialog
        await Page.ClickAsync("#promptButton");
        textContent = await statusLocator.TextContentAsync();
        Assert.That(textContent, Is.EqualTo("You pressed Cancel"));

        Page.Dialog += PromptDialogHandler;
        await Page.ClickAsync("#promptButton");
        Page.Dialog -= PromptDialogHandler;
        textContent = await statusLocator.TextContentAsync();
        Assert.That(textContent, Is.EqualTo("You returned: sure"));

        return;

        static void AlertDialogHandler(object? sender, IDialog dialog)
        {
            Assert.That(dialog.Message, Is.EqualTo("Alert"));
            dialog.DismissAsync();
        }

        static void ConfirmDialogHandler(object? sender, IDialog dialog)
        {
            Assert.That(dialog.Message, Is.EqualTo("Press OK or Cancel"));
            dialog.AcceptAsync();
        }

        static void PromptDialogHandler(object? sender, IDialog dialog)
        {
            Assert.That(dialog.Message, Is.EqualTo("Please enter a response"));
            dialog.AcceptAsync("sure");
        }
    }
}