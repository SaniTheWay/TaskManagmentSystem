Hello, This is a Task Managment System - offers comprehensive features to help you plan, track, and deliver projects efficiently.

Here is what are the feature and how to Use it 

1. For Local Setup you need a visual studio / sql server and ssms

-Download this project or clone repo and in appsetting.json
![image](https://github.com/user-attachments/assets/dc64c1a4-bb0d-4b5b-bb38-8becfc50d784)
-Add you server name or connection string 

-After this add a migration or Update-Database (Add migration if project does not contain latest one in -project/migration folder)
-Onces DataBase is updated - you are all set to go 

2. Feature and Useage 

- when You gonna run project then first you gonna see is login screen , if you have an account then login otherwise register and then login 

![image](https://github.com/user-attachments/assets/0d95de2e-b079-412a-ae6b-aca7ea9d287a)
![image](https://github.com/user-attachments/assets/944e197b-6fe9-48ff-b2b0-d628b9c19329)

------ Note ------ when you signUp the the by default role you get is of "TeamMember" 

-- in TeamMember/TeamLead role after login you will be redirected to GenericDashboard where you can see your and your TeamMates tasks 
-- in CompanyAdmin role after login you will be redirected to AdminDashboard where you can see All Task , you task , overall report and task divided by due date

And to be CompanyAdmin you need to run - Update Users set UserRole = 'CompanyAdmin' where UserId = 2 - (replace 2 with userid you want to make CompanyAdmin) in sql in users table

- After login - you will be redirected to the Dashboard , for TeamMember/TeamLead it will look like this -- (lets go with TeamMember/TeamLead senario dirst)

![image](https://github.com/user-attachments/assets/339091f0-f339-4d1e-85bf-37c9372cc6c7)

- You are new user so you won't see any task any assigned to you and you are not into a team yet so wont see any task in teammember column also
- There are 3 option  Add team , Add TeamMember and Add Task 
![image](https://github.com/user-attachments/assets/eaa424ab-5b0d-4d49-a753-a0c16f4fe529)

- First add a team - and if any team exist already you can add yourself into that team also by just click on add member 
- To add a team - click on add team button you will see this 
![image](https://github.com/user-attachments/assets/2c9ab3e7-9f6f-49e0-98c6-35624cd531b1)

- Give Name to Team and select member you want to add to this teamand click on Save , Team Will be added right away 
![image](https://github.com/user-attachments/assets/ca2ebb2d-3465-4541-b397-a2959f70f626)

- Now if you want to add more member to this team , then click on Add Member then select this team and add member 
![image](https://github.com/user-attachments/assets/b08baa8d-dfb0-4725-b51e-7a44b207313f)


- You can now add task , for that click on add task button and then - add details like title , desc , team that task come under and after selecting team you will only see member that comes under that team , if you dont see any member after selecting a team then add member to that team with add member button , then add due date and you can add one attachment (you can add more attachments and notes after the task is create) and click on Add button -- (All newly created tasks will be in ToDo status)
![image](https://github.com/user-attachments/assets/173be9db-a99f-4b93-940f-987481ed89e6)

- After Task Created Sucessfully you will redirected to Dashboard where you will be able to seetask created by you if you have assigned that task to youself or one of your teammates
![image](https://github.com/user-attachments/assets/dba34d60-8277-4454-83a4-b21f6363bc10)

- same way try to assign task to you teammate and if will be visible under My teammate tasks 
![image](https://github.com/user-attachments/assets/d0a74f16-0cf9-41b9-83b4-9108efa4d2b2)


-- now if you click on any of the task card you will redirected to task detail screen - which look like this
![image](https://github.com/user-attachments/assets/6157d2fd-2bad-4e3c-b67b-43d84de2444c)
here you have title , description and then you have two accordion Attachments and notes which contains all the attachments and notes this task have 
![image](https://github.com/user-attachments/assets/04d98e7c-2b6e-4658-9e0c-a3a59b69292e)
![image](https://github.com/user-attachments/assets/0594ee22-f0c8-4979-8730-706b30c7dfe0)

You can Click on down in attachment to download the attachments

You also have 3 more feature , which is Status , edit task and add attachments
![image](https://github.com/user-attachments/assets/7c494ab1-0397-4918-8dbd-a8c6c538d595)

-- with Edit Task you can edit the task , change assignee , duedate , title , desc
![image](https://github.com/user-attachments/assets/6b74a310-5e77-4223-b969-9b18e2705eec)

-- With add attachments you can attach more attachments 

![image](https://github.com/user-attachments/assets/50119552-e49a-46fe-b541-2ca72306478f)
![image](https://github.com/user-attachments/assets/de3875e3-121d-4573-95b9-8d6a0f96bc03)

-- with status button , when you gonna hover over this button you will see different options like 
![image](https://github.com/user-attachments/assets/684be523-af59-46c4-91b1-e3f25be4e2cd)

select new status to move card into different stage - like i choose QA then it move to QA stage
![image](https://github.com/user-attachments/assets/0254c457-4288-43db-b5d9-e10f83104dbf)
![image](https://github.com/user-attachments/assets/05de36c9-06d4-4a1e-b060-e9291b8f73c6)

and you can add notes also 
![image](https://github.com/user-attachments/assets/772260f0-f4b9-4c57-916f-aa69778eeeb9)
![image](https://github.com/user-attachments/assets/7970d380-59ab-4746-88ff-d6a02ea88689)

-------- noe coming back to CompanyAdmin -- if you set up you role as CompanyAdmin via Update Users set UserRole = 'CompanyAdmin' where UserId = 2 -----------
and then you log in with that credentials then you will see AdminDashboard instead GenericDashBoard Which will look like this
![image](https://github.com/user-attachments/assets/f8da3f7a-11e4-4cb0-93d3-596cff77704f)

it have to same facilities as generic ![image](https://github.com/user-attachments/assets/1e308cc5-6b54-4f13-ac6a-a5e847ebccc1)

but you can see lot more , like my task ,  All task , completed tasks , Exceeded due date , early due date , late due date , overall report in according form

![image](https://github.com/user-attachments/assets/62168846-fa8d-4ea2-b8eb-9573e7a62375)
![image](https://github.com/user-attachments/assets/1870a336-a584-4c5f-a226-c9ab3f1a6de0)
![image](https://github.com/user-attachments/assets/273f6513-3d3a-498f-9b8e-1dcc252d2336)
![image](https://github.com/user-attachments/assets/ff1401ff-4f26-46e2-bd29-9d12628f8579)
![image](https://github.com/user-attachments/assets/479123f1-f77f-4ee3-94c2-0f612c3b2f3e)

You can also create task add team , add member , add notes and attachments , view card , change status , edit card as usual

-- Also HAve a little logout button on the top 
![image](https://github.com/user-attachments/assets/f24524be-f7da-468c-8a04-56d8e1aa5ecd)


And that's about it , Enjoy :)

------- Note :- I have Created this in 2 days (incuding documentation :)) so it may have many unnoticed bugs and problems ---- if you find one please feel free to report on ranavivek367@gmail.com 
