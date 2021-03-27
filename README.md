# How To's
This Project consists of a collection of Web Applications that allow you to manage, edit and view How To's in the form of Step-By-Step procedures. An important key aspect is the modularity of the documentation. Reusable steps, pictures, explanations, links and so on are a core concept and the database is designed with that in mind.

## Web Applications Diagram
![](docs/howtos_server.png?raw=true "How To's server diagram")

### Core API
The REST API that handles all database interactions on the documentation database.

### Administration
A Website that allows users to manage the content. Consumes the Core API.

### Viewer
A Website that allows users to view the How To's. Consumes the Core API.

## Continuous Integration Diagram
![](docs/howtos_server.png?raw=true "How To's server diagram")

## UML
![](docs/howtos_ci.png?raw=true "How To's CI diagram")


## API Core Features
* RESTful witch JSON payload
* HTTPS over SSL
* API versioning preparation
* Beautiful URLs
* GET ressources with required content
* GET pictures as links (ToDo: How to serve static files?)
* GET complete How To / Superstep tree with links to the steps (navigation in Viewer, Administration)
* GET should include links to previous and next step
* POST for content creation
* PATCH for content update
* DELETE with recycle bin (Restoring may be a bit tricky, because the content is very stricktly linked)
* Basic authentication (Over access token?)


## Documentation Core Features
Depending on the complexity and time of the project various features can be implemented. Sorted by priority descending.
* How To's include steps, sortable
* Steps used as Supersteps (wich linked Substeps) or Steps (with explanations), sortable
* Steps and Supersteps reusable (used by various Supersteps / How To's)
* How To's, Supersteps, Substeps have content (title, description, notes, to do's), depending on what makes sense
* Steps have Explanations
* API reference not by Database id but specific string or number (beautify URLs)
* Explanations have content (title, description, note, to do's)
* Explanations contain Explanation Modules
* Explanation Module Text
* Explanation Module Code
* Explanation Module Text contain links
* Explanation Module Text contain pictures (Depending on the Editor Framework -> research required
* Link can be external or internal
* Internal / External links can be managed
* Module Knowledge Base
* Module Knowledge Base contains Explanations
