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
public class ClickTest : PageTest
{
    [SetUp]
    public async Task TestInitialize()
    {
        Page.Context.SetDefaultTimeout(20_000);
        await Page.GotoAsync(AppConfig.Get("TestPage"));
    }

    [Test]
    public async Task RadioAndCheckboxTest()
    {
        var amLocator = Page.Locator("#am");
        await amLocator.ClickAsync();
        Assert.That(await amLocator.IsCheckedAsync(), Is.True);
        var fmLocator = Page.Locator("#fm");
        Assert.That(await fmLocator.IsCheckedAsync(), Is.False);
        var checkboxLocator = Page.Locator("#checkbox");
        Assert.That(await checkboxLocator.IsCheckedAsync(), Is.True);
        await checkboxLocator.ClickAsync();
        Assert.That(await checkboxLocator.IsCheckedAsync(), Is.False);
        await checkboxLocator.ClickAsync();
        Assert.That(await checkboxLocator.IsCheckedAsync(), Is.True);
    }
}
