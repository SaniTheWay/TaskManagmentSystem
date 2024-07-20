Hello, This is a Task Managment System - offers comprehensive features to help you plan, track, and deliver projects efficiently.

Here is what are the feature and how to Use it 

1. For Local Setup you need a visual studio / sql server and ssms

-Download this project or clone repo and in appsetting.json
![image](https://github.com/user-attachments/assets/78961273-95ed-419b-ab76-2c19d460541e)

-Add you server name or connection string 

-After this add a migration or Update-Database (Add migration if project does not contain latest one in -project/migration folder)
-Onces DataBase is updated - you are all set to go 

2. Feature and Useage 

- when You gonna run project then first you gonna see is login screen , if you have an account then login otherwise register and then login 

![image](https://github.com/user-attachments/assets/887d7d1f-be83-455f-b3f5-8633961b1d96)

![image](https://github.com/user-attachments/assets/36be4615-a5f1-4c57-8a8a-8ebbd31e8f81)


------ Note ------ when you signUp the the by default role you get is of "TeamMember" 

-- in TeamMember/TeamLead role after login you will be redirected to GenericDashboard where you can see your and your TeamMates tasks 
-- in User Admin role after login you will be redirected to AdminDashboard where you can see All Task , you task , overall report and task divided by due date

And to be User Admin you need to run - Update Users set UserRole = 'CompanyAdmin' where UserId = 2 - (replace 2 with userid you want to make CompanyAdmin) in sql in users table

- After login - you will be redirected to the Dashboard , for TeamMember/TeamLead it will look like this -- (lets go with TeamMember/TeamLead senario dirst)

![image](https://github.com/user-attachments/assets/c8e90537-953e-4ef5-a45c-bf59c6aa88db)


- You are new user so you won't see any task any assigned to you and you are not into a team yet so wont see any task in teammember column also
- There are 3 options in the **Action** dropdown - Add team , Add TeamMember and Add Task 
![image](https://github.com/user-attachments/assets/de1f33c8-e2d0-4baf-a00e-65dc1f466801)


- First add a team - and if any team exist already you can add yourself into that team also by just click on add member 
- To add a team - click on add team button you will see this 
![image](https://github.com/user-attachments/assets/956dbb0c-e563-4720-9058-46eff92cb3f2)

- You can now add task , for that click on add task button and then - add details like title , desc , team that task come under and after selecting team you will only see member that comes under that team , if you dont see any member after selecting a team then add member to that team with add member button , then add due date and you can add one attachment (you can add more attachments and notes after the task is create) and click on Add button -- (All newly created tasks will be in ToDo status)
![image](https://github.com/user-attachments/assets/3f7d052b-191f-4ade-bde2-eb82e2d8ca6f)

- After Task Created Sucessfully you will redirected to Dashboard where you will be able to seetask created by you if you have assigned that task to youself or one of your teammates
- Same way try to assign task to you teammate and if will be visible under My teammate tasks 

- Now if you click on any of the task card you will redirected to task detail screen - which look like this
![image](https://github.com/user-attachments/assets/0470b32b-3eb1-42bc-8414-78deefb610c8)

here you have title , description and then you have two accordion Attachments and notes which contains all the attachments and notes this task have 
![image](https://github.com/user-attachments/assets/1518cff4-828c-4b64-99ba-4f04427b304f)

You also have 3 more feature, which is Status, edit task and add attachments
![image](https://github.com/user-attachments/assets/de20e147-7831-41ef-a985-18603667ae8b)

-- with Edit Task you can edit the task , change assignee , duedate , title , desc
![image](https://github.com/user-attachments/assets/5216f425-2e54-4e6e-9ac9-793b6941131e)

-- With add attachments you can attach more attachments 

![image](https://github.com/user-attachments/assets/b5c7631b-9c05-4f30-b679-fa330fb24ecd)


-- with status button , when you gonna hover over this button you will see different options. Select new status to move card into different stage and you can add notes also 
![image](https://github.com/user-attachments/assets/b4ae2651-19be-49b9-a64e-2ab74f0a60ed)


-------- now coming back to User Admin -- if you set up you role as User Admin via Update Users set UserRole = 'CompanyAdmin' where UserId = 2 -----------
and then you log in with that credentials then you will see AdminDashboard instead GenericDashBoard Which will look like this
![image](https://github.com/user-attachments/assets/1740de2a-ea21-4812-8fed-5b9ec472f04e)


it have to same facilities as generic dashboard but you can see lot more things, like:  my task ,  All task, completed tasks, Exceeded due date, early due date, late due date, overall report in according form.
![image](https://github.com/user-attachments/assets/4119b7c1-2e91-41f0-be5f-c54f77debed1)
![image](https://github.com/user-attachments/assets/ddc5d68b-5ab6-48c6-ab19-7636a9aa41cb)


You can also create task add team , add member , add notes and attachments , view card , change status , edit card as usual

-- Also Have a little logout button on the top 
![image](https://github.com/user-attachments/assets/b0425ae0-3d13-4db7-ad5c-49e8ecbd540e)

Thanks and Regards,
Jasper Ashwin
