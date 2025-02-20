# Aiury's Weather Suggestions

Aiury's Weather Suggestions is a serverless .NET 8.0 C# Lambda program that provides personalized activity advice based on the user's current weather and location. By leveraging AWS Lambda and API Gateway, the application dynamically fetches weather data to generate tailored activity suggestions.

## Features

- **Personalized Activity Advice:** Get recommendations based on the user’s location and current weather.
- **Serverless Architecture:** Built on AWS Lambda and API Gateway for scalability and cost-effectiveness.
- **Clean and Maintainable Code:** Developed using SOLID principles and modern C# practices.
- **Multi-Culture Support:** Generates advice in different languages (e.g., PT-BR, EN-US, ES-ES).

## Technologies Used

- .NET 8.0
- C#
- AWS Lambda & API Gateway
- Amazon.Lambda.AspNetCoreServer
- HTTPClient & Newtonsoft.Json for API communication

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [AWS CLI](https://aws.amazon.com/cli/) (configured with appropriate credentials)
- [AWS Lambda Tools for .NET CLI](https://github.com/aws/aws-lambda-dotnet)
- An AWS account with permission to create and manage Lambda functions and API Gateway resources.

## Geolocation Considerations
### IPService for Testing:
In this project, the IPService is implemented to determine the location based on the IP address. This approach is useful for testing and demonstration purposes, as it provides a fallback location when no geolocation data is provided.

Real-World Scenarios:
In real-world usage, relying on IP-based geolocation is not ideal because:

It determines the server’s (or proxy’s) location rather than the end user’s actual location.
Accuracy can be limited or completely off depending on the network configuration.
For production scenarios, it is recommended that your front-end (or client application) retrieves the end user's geolocation using browser APIs (e.g., navigator.geolocation) and then sends these coordinates (latitude and longitude) to your Lambda endpoint. This will ensure that the advice provided is truly personalized based on the user's actual location and weather conditions.
