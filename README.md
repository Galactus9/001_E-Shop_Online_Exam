## E-Shop and Online Examination


This project is a fully functional web application that includes an E-Shop and an online examination process. 
It aims to cater to certification companies by providing them with a platform to sell their products and conduct online examinations.


## Scope of the project

The application is designed to address the needs of certification companies that require an E-Shop for selling their certification 
materials and an online examination process. It serves as a simplified solution to meet these requirements.

### E-Shop: 
The E-Shop component provides an online platform where certification companies can sell their products. 
This includes certification materials, study guides, practice exams, and other related items. 
The E-Shop incorporates essential features such as product listing, product details, shopping cart, 
payment integration, order management, and user authentication.


### Online Examination Process: 
The online examination process focuses on offering a digital platform for administering exams.
Certification companies can utilize this process to conduct exams for individuals seeking certification. 
Key features of the online examination process include exam creation and configuration, question bank management, 
exam scheduling, exam delivery to candidates, answer submission, result calculation, and result reporting.



## Technologies used.
The project utilizes the following technologies:

- Asp .Net Core 6: The web application framework used for building the application.
- MVC: The architectural pattern employed for structuring the application.
- iTextSharp: A library used for generating PDF documents.
- Stripe: A payment processor integrated into the E-Shop for secure online transactions.
- SignalR: A library used for real-time messaging between the application and users.
- jQuery: A JavaScript library used for enhanced interactivity and dynamic content.



## How to run the application

 - Step 1: Install Prerequisites

    Make sure you have the .NET Core 6 SDK installed on your machine. You can download it from the official .NET website.   
    Also you will need SQL Server Management Studio (SSMS).You can download it from the official .NET website.
    
 - Step 2: Clone or Download the Application
 - Step 3: Open the Project
 - Step 4: Restore Dependencies
    Open a terminal or command prompt and navigate to the project's root directory.
    Run the following command to restore the project dependencies: dotnet restore
 - Step 5: Build the Project
    dotnet build
 - Step 6: Run the Application
    dotnet run
    
    
   When you run the application a database called E-ShopOnlineExam will be created if not exist.
   
## User experience

  There are 4 diferenent roles in this application.
  - Candidate(User)
  - Marker
  - Quality Controll
  - Admin
  
### User's scope

  As a user first you need to register an account or login.
  
  ![Screenshot (2)](https://github.com/Galactus9/001_E-Shop_Online_Exam/assets/117937168/0df42163-227e-4abf-a41d-c17c69bd511a)

  ![Screenshot (3)](https://github.com/Galactus9/001_E-Shop_Online_Exam/assets/117937168/08edfc66-886c-459a-bc3c-a0e96d3c449b)
  
  Then you can buy a certificate.
  
  ![Screenshot (4)](https://github.com/Galactus9/001_E-Shop_Online_Exam/assets/117937168/41261598-b226-4582-999f-e025af2bce2e)  
  ![Screenshot (5)](https://github.com/Galactus9/001_E-Shop_Online_Exam/assets/117937168/2186e468-1afb-43e5-9c92-00c85819648d)
  
  To procede to checkout use as cart number: 4242 4242 4242 4242
  
  ![Screenshot (6)](https://github.com/Galactus9/001_E-Shop_Online_Exam/assets/117937168/c91ca8a2-8e13-4b41-8c10-11348abb27dc)

  Then inside MyAccount Section you can find and schedule an exam.
  If you press start an examination prosses going to run.
  
  ![Screenshot (7)](https://github.com/Galactus9/001_E-Shop_Online_Exam/assets/117937168/d1e1b5f7-90a2-4ffd-820b-7690ccf96024)

  In case you get disconnected from your examination, the system can provide you the ability to reconnect and continue the prosses.
  
  Ιf you had answered questions, they will be probably saved once the program gets a timestamp of your examination every 10 seconds.

  When you submit your answers, you will be able to download your certificate results in PDF.
  
  In the user’s section, you will be able to change your role to see the full functionality of the application.

  
  ![Screenshot (9)](https://github.com/Galactus9/001_E-Shop_Online_Exam/assets/117937168/c1993c27-4530-4235-b758-de40b170b409)

  As a Quality control, you will be able to report bugs and ideas using the book in the top right corner.

  Those reports will appear to Admins under the Manage Account section inside the Reports. 

  That feature will appear to you only in case you are an Admin.
  
  <br/>
  
  In Admin Actions you can find and proceed with CRUD operations,
  
  The to-do list at the bottom of Admin Actions is a section where you can assign a marking task to a marker.
  
  When a task is assigned, the marker can find it in Marker Actions.
  
  By the moment the marker manages the task will wait in the Admins Actions for acceptance. 
  
  In case the admin accepts the marker’s work the task will be characterized as closed,

  If deny the task will return to the marker to do it again.
  
  ![Screenshot (11)](https://github.com/Galactus9/001_E-Shop_Online_Exam/assets/117937168/69bb2010-06fe-41e8-95e9-896c2b91d8ef)

