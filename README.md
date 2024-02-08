# E-commerce Product Review Management System Based on Opinion Mining

This project presents a semi-supervised approach for mining online user reviews to generate comparative feature-based statistical summaries, aiding users in making informed online purchase decisions. It involves preprocessing, feature extraction and pruning, feature-based opinion summarization, and overall opinion sentiment classification. Our method demonstrates acceptable accuracy in identifying opinionated sentences from user reviews, leveraging techniques such as POS tagging and sentiment analysis with SentiWordNet.

## Features

- Web scraping for real-time user reviews
- Parts-of-speech tagging for review analysis
- Feature extraction for identifying key aspects of products
- Opinion summarization to generate comparative insights
- Sentiment classification to discern overall user sentiment

## Technologies

- ASP.NET for the backend
- Microsoft SQL Server for data management
- SentiWordNet for sentiment analysis
- Visual Studio as the development environment

## Setup and Installation

1. **Install MySQL Server 2012**: Download and install from the official MySQL website.
2. **Install Visual Studio 2013**: Download and install from the official Visual Studio website.
3. **Database Setup**:
   - Use the provided DB script to set up the SQL database.
   - Update the database path in `site.master` as per your setup.
4. **Project Compilation**:
   - Open the project in Visual Studio 2013.
   - Build the project to compile it.
5. **Running the Application**:
   - Start the application by running `login.aspx` from Visual Studio.


## Contributing

We welcome contributions to improve the project. Please fork the repository and submit a pull request for review.

## License

This project is licensed under the MIT License - see the LICENSE.md file for details.

## Acknowledgements

- Our mentors and advisors for guidance
- The open-source community for various tools and libraries used

For detailed documentation and methodology, refer to our comprehensive report included in the repository.
