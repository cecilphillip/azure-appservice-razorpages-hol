Publishing your first Razor Pages application to Azure
========================================================

## Introduction

The release of ASP.NET Core 2 brought .NET developers many new features for building web applications. For a comprehensive list of updates, you can read those the [release notes](https://github.com/dotnet/core/blob/master/release-notes/2.0/2.0.0.md). The feature that we will be focusing on in this lab is the new [Razor Pages](https://docs.microsoft.com/en-us/aspnet/core/mvc/razor-pages/?tabs=visual-studio-code) programming model. With Razor Pages, we have a new page-first structure that allows you to focus on the user-interface and simplify the server-side experience. It is built on top of the existing MVC APIs in ASP.NET Core and uses the same Razor view engine.

The goal of this lab is to show you how to create a new ASP.NET Core application using the Razor Pages programming model, and then publish your applicaiton into an Azure AppSerivce. The instructions provided should work consistently across operating systems. Therefore, you should be able to complete this lab whether you're on Windows, Linux, or MacOS.

Before starting, you wil need to make sure you have the following tools and accounts setup and ready.

1. [Visual Studio Code](https://code.visualstudio.com/)
2. [.NET Core 2.0 SDK](https://www.microsoft.com/net/download)
3. [Git commandline tools](https://git-scm.com/download)
4. [GitHub account](https://github.com/)
5. [Microsoft Azure account](https://azure.microsoft.com/free)

If you ever get stuck with the lab exercises, feel free to review the completed solution inside of the **final** folder.

<a name="Exercises"></a>
## Exercises
This workshop consists of the following exercises:

1. [Create a new Razor Pages Application](#Exercise1)
2. [Install the C# extensions for Visual Studio Code](#Exercise2)
3. [Add a web form](#Exercise3)
4. [Accept form input](#Exercise4)
5. [Push your code to GitHub](#Exercise5)
6. [Create a Web App in Azure](#Exercise6)
7. [Setup GitHub Deployment](#Exercise7)

<a name="Exercise1"></a>
## 1. Create a new Razor Pages Application
The first thing you will do is create a new Razor Pages application using the dotnet CLI. If you have installed the .NET Core 2 SDK, then the CLI should already be available on your machine. If you haven't, head over to the [.NET page](https://dot.net) to download and install a copy of the .NET SDK for your operating system.

### Steps
1. Create a new folder for the application on your machine.
2. Open a command prompt and navigate to the folder.
3. Enter the following command to generate a new Razor Pages web application: `dotnet new razor -n MyRazorWebApp`.
4. While still in the command prompt, enter `dotnet run` to run the generated.
5. Go to your web browser, and enter `http://localhost:5000` in the address bar to see your applicaiton.

<a name="Exercise2"></a>
## 2. Install the C# extensions for Visual Studio Code
Visual Studio Code is a great editor by itself. However, if you would like to be able to debug and run your C# code within Visual Studio Code, you will need to install some extensions.

### Steps
1. Inside of Visual Studio Code, click on the extensions icon in the left vertical menu.
2. Once the `Extensions` section is open, search for C#.
3. Install the [C# extension from Microsoft](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp).
4. Also install the [C# IDE extenions from jchannon](https://marketplace.visualstudio.com/items?itemName=jchannon.csharpextensions).
5. You will need to restart Visual Studio Code to complete the installation.

<a name="Exercise3"></a>
## 3. Add a web form
The generated applicaiton is created with the default ASP.NET template. It includes things like jQuery, bootstrap and some basic CSS styling. It also includes a few pages that you can navigate through. In this exercise, you are going to update the Contact page by adding a form to accept user data.

### Steps
1. Open the folder that holds your generated application in Visual Studio Code.
2. Inside your project folder, create another folder called `Models`.
3. Right click on the `Models` folder. Select `New C# class` and name it `Contact`.
4. Update the `Contact` class to include the properties shown below.

```csharp
 public class Contact
 {
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Note { get; set; }
 }
```

5. Next, expand the `Pages` folder and click on `Contact.cshtml.cs`
6. In the `ContactModel` class, add a Contact property of type `Contact`.

```csharp
public Contact Contact { get;set; }
```

7. Now click on `Contact.cshtml` under the `Pages` folder in Visual Studio Code.
8. Update the markup in this file to include the following form.

```html
@page
@model ContactModel
@{
    ViewData["Title"] = "Contact";
}
<h2>@ViewData["Title"]</h2>
<h3>@Model.Message</h3>

<form method="POST">
    <div asp-validation-summary="All"></div>
    <div class="form-group">
        <label asp-for="Contact.Name"></label>
        <input asp-for="Contact.Name">
    </div>

    <div class="form-group">
        <label asp-for="Contact.Email"></label>
        <input asp-for="Contact.Email">
    </div>

    <div class="form-group">
        <label asp-for="Contact.Note"></label>
        <textarea asp-for="Contact.Note"></textarea>
    </div>
    <div class="form-group">
        <button class="btn btn-primary">Submit</button>
    </div>
</form>
```

9. Let's run the code to see what we have so far. In Visual Studio Code, click on the `Debug` item in the top horizontal menu then select `Start Debugging`.
10. Navigate to `http://localhost:5000/contact` in your web browser to see your form.

<a name="Exercise4"></a>
## 4. Accept form input
At this point, the form should be displaying in the web browser but no data is being received on the server-side. With Razor Pages, you can add *handler methods* to process requests associated with the various HTTP verbs. The name of the handler method is based on the HTTP verb + the `On` prefix. For example, requests made with an HTTP POST will be handled by an `OnPost` handler. Optionally, the `Async` naming suffix can be used for asynchronous handlers (`OnPostAsync`).

### Steps
1. Go to `Contact.cshtml.cs` in Visual Studio Code.
2. Add a `[BindProperty]` attribute on top of the Contact property to bind it to the form data.
3. Next you will add a OnPost handler to create a thank you message for the user.
4. Afterwards, redirect the user to the Contact page.
5. Add a `[TempData]` attribute to the Message property to persist the message between requests.
6. Also, check to see if the `ModelState` is valid before creating the message.
7. Your completed code should look similar to the example below.

```csharp
public class ContactModel : PageModel
{
    [TempData]
    public string Message { get; set; }

    [BindProperty]
    public Contact Contact { get;set; }

    public IActionResult OnPost() {
        if (!ModelState.IsValid) {
            return Page();
        }
        Message = $"Thank you {Contact.Name} for contacting us. We will get back to your shortly";
        return RedirectToPage();
    }
}
```

8. Run your application again from the Debug menu. Try submitting the form to see the results.

<a name="Exercise5"></a>
## 5. Push your code to GitHub
Your simple Razor Pages application is ready to be deployed. The following exercises will guide you through automating deployment to Azure using GitHub.

Before doing that, you will need to push your application code into a GitHub repository. If haven't setup an account on GitHub, go to the [sign up page](https://github.com/join) and register for a free account.

### Steps
1. Go to the GitHub website, click on the `+` button in the top navigation, and choose `New repository`.
2. Fill out the form to create a public repo. Optionally, you may choose to add a `.gitignore` file for Visual Studio.
3. After you've clicked the `Create repository` button, you will be taken to the main page for your repository.

![](media/create_repo.png)

4. Click on the `Clone or download` button, and copy the URL that's in the text box. This URL will needed to sync your code with GitHub.
![](media/clone_url.png)
5. Go back into Visual Studio Code where you have been working on your code.
6. In the top horizontal menu, click on `View` then select `Integrated Terminal`.
7. Once the terminal opens, enter the following command to initialize git in your application folder: `git init`.
8. Next, use the URL from step 4 to connect your repository in GitHub with your local git folder:  `git remote add origin <<replace-with-your-git-url>>`. Make sure you use your own GitHub URL.

![](media/git_init.png)

10. Now you have to let git know about the files you want to add. Enter the following command in the terminal:  `git add .`.
11. Commit your files and add a meaningful message with this next command: `git commit -m "adding my code"`.
12. Finally, push your code to the repository in GitHub: `git push origin master`.

<a name="Exercise6"></a>
## 6. Create a Web App in Azure
Azure offers a few options for hosting web applications in the cloud. This exercise will walk you through creating an [Azure Web App](https://azure.microsoft.com/en-us/services/app-service/web/). Azure Web Apps enables you to build and host web applications in the programming language of your choice without managing infrastructure. It supports both Windows and Linux, and enables automated deployments from various services.

### Steps
1. Navigate to the Azure portal in your web browser.
2. Click on the `New` button, choose `Web + Mobile` then choose `Web App`.

![](media/create_web_app.png)

3. Enter the name for your web app, and also a name for your resource group.
4. Next, click `Create` and wait a moment for your Azure Web App instance to be created.
5. Go to your newly Web App in the Azure portal, and click on it's URL.
![](media/web_app_portal.png)
6. You should see the site load with a welcome message, similar to the image shown below.

<a name="Exercise7"></a>
## 7. Setup GitHub Deployment
With your code in Github and a Web App created in Azure, the last thing to do is connect the two together.

### STEPS
1. In the Azure portal, go to your Web App
2. In the left side menu of your app, choose `Deployment options`.
3. Choose GitHub as the source. You will need to enter your GitHub credentials.
4. Under `Choose project`, select the name of your repository.
5. Click `Ok`. Azure will pull your code from GitHub and deploy it.
6. Go back to `Deployment options` at any time to see the progress.
7. Once the deployment is complete, navigate to your Web App's URL to see the results.
8. Pushing new code changes to the `master` branch of your GitHub repository will now trigger a new deployment.


## References
In this lab, you
* [ASP.NET Core Razor Pages documentation](https://docs.microsoft.com/aspnet/core/mvc/razor-pages/?tabs=visual-studio)
* [Azure Web Apps documentation](https://docs.microsoft.com/azure/app-service/)
* [Deploy an ASP.NET Core app to Azure App Service](https://docs.microsoft.com/aspnet/core/tutorials/publish-to-azure-webapp-using-cli?tabs=other)
* [Razor Pages videos on Channel9](https://channel9.msdn.com/Search?term=%22Razor%20Pages%22#pubDate=year&ch9Search&lang-en=en)