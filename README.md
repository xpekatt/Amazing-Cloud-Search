<<<<<<< HEAD
WARNING [Beta] - API-2013
=======
Amazing Cloud Search - C# API For Amazon Cloud Search
============

Allow you to search, faceted search, add, update, remove objects from your Amazon Cloud Search Index in C#.
For using the 2013 API [see the 2013-API branch.](https://github.com/martin-magakian/Amazing-Cloud-Search/tree/2013-API "Title") 


Contribute
---------
Amazon CloudSearch is a AWS library created and maintain by [Anomaly Detection](https://anomaly.io)<br />
Feel free to contribute to the project using push request.

How to use
>>>>>>> refs/remotes/martin-magakian/master
---------

I recieved this big pull request from Tyler. It's suppose to work with Amazon CloudSearch API 2013.
Unfoturnly I never had time to test this code.

So try at your own risk ;-)
Let me know if you can use this project.

<<<<<<< HEAD

=======
Contact
=========
Developed by Martin Magakian dev.martin.magakian@gmail.com<br />
by [Anomaly Detection](https://anomaly.io)
>>>>>>> refs/remotes/martin-magakian/master



EXTRA
-------

In the following email Tyler explain me a little bit about his work:

<<<<<<< HEAD

> 	Hi Martin,
> 
> 	Here are my changes.  The biggest change is in the QueryBuilder.cs file and the FeedBooleanCritera method.  We have several possible combinations of and'ing and or'ing lists and > individual items together.  Also, amazon is very very picky about exact spacing and nesting.  The method is bug free because we are actively using the library with many different > boolean queries.  The one thing this does now that we really needed was and'ing together several or'ed lists.
> 
> 	For example:
> 
> 	bq=(and+artist:100001+categories:'67'+categories:'33'+(or+type:1+type:2))
> 
> 	here is an or'ed list and an and'ed list.
> 
> 	bq=(and+artist:100001+categories:'67'+categories:'33'+(and+(or+type:1+type:2)+(or+quality:'1'+quality:'2'+quality:'4'+quality:'5')))
> 
> 	here are two or'ed lists and'ed.
> 
> 	The one thing library is missing and I would like to build at some point is url enconding for input parameters.  It would also be great if json serialization could come from .Net > and to drop the postsharp so that there was just 1 single library to include.
> 
> 	Thanks again for all your great work, we got a huge jump start on the project thanks to your library.  I think you could get a LOT of use of this library because its the best one > available right now for .Net and CloudSearch.
=======
>>>>>>> refs/remotes/martin-magakian/master
