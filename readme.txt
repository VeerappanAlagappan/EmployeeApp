Welcome to Employee App
-------------------------

This App would expect an XML with the below schema and based on this it can add new employee elements/delete the existing employees


<employees>
  <employee>
    <name>Ram</name>
    <age>30</age>
    <designation>Software Engineer</designation>
  </employee>
  <employee>
    <name>Rahim</name>
    <age>40</age>
    <designation>Senior Developer</designation>
  </employee>
  <employee>
 </employees>
 
 To implement this solution, I have used the below best practices
 i) Followed Dependency Injection
 ii)Used Design Patterns: Singleton,Command
 iii)Used Reflections