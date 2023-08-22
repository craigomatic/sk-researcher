# sk-researcher
Using Semantic Kernel to power a research agent that will work on your behalf to research a topic

## Pre-requisites
You will Azure OpenAI or OpenAI keys. In addition it's recommended that you have a deployed version of the [websearch-plugin](https://github.com/craigomatic/websearch-aiplugin/) and [webscraper-plugin](https://github.com/craigomatic/webscraper-aiplugin/) available 

## Usage
Assuming you will be running the app locally, enter your keys and the path to the websearch-plugin and webscraper-plugin into appsettings.json (or even better, create a copy of appsettings.json and rename it to appsettings.development.json).

Then edit the research request Program.cs to the topic you would like to research and run the code!

<img width="915" alt="image" src="https://github.com/craigomatic/sk-researcher/assets/146438/2af6a570-c729-44a9-aa88-4c159175b5de">

### Running on Azure
The configuration convention is a little tricky when it comes to collections, to reference your plugins you will need to follow this pattern:

```javascript
name: 'AppConfig:Plugins:0:Name'
value: 'WebSearch'

name: 'AppConfig:Plugins:0:Url'
value: '<plugin url goes here>'

name: 'AppConfig:Plugins:1:Name'
value: 'WebScraper'

name: 'AppConfig:Plugins:1:Url'
value: '<plugin url goes here>'

//the next plugin would use the number 2, and so on...
```
