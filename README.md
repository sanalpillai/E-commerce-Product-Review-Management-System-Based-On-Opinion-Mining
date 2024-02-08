# E-commerce Product Review Management System Based on Opinion Mining

This project presents a semi-supervised approach for mining online user reviews to generate comparative feature-based statistical summaries, aiding users in making informed online purchase decisions. It involves preprocessing, feature extraction and pruning, feature-based opinion summarization, and overall opinion sentiment classification. Our method demonstrates acceptable accuracy in identifying opinionated sentences from user reviews, leveraging techniques such as POS tagging and sentiment analysis with SentiWordNet.

## Features

- Web scraping for real-time user reviews
- Parts-of-speech tagging for review analysis
- Feature extraction for identifying key aspects of products
- Opinion summarization to generate comparative insights
- Sentiment classification to discern overall user sentiment

## Key Techniques and Technologies

- **Web Scraping**: Automated scraping of product reviews from e-commerce websites.
- **Natural Language Processing (NLP)**: Utilizes parts-of-speech (POS) tagging for preprocessing and analysis of the review text.
- **Feature Extraction**: Identification of key product features from reviews using NLP techniques. The extraction process treats frequently occurring nouns and noun phrases as potential features, with associated adjectives indicating sentiment orientation.
- **Opinion Summarization and Classification**: Uses SentiWordNet for sentiment analysis to classify the overall sentiment of reviews. The system generates feature-wise summaries and comparative insights among different products.
- **Technologies**: Developed using ASP.NET, Microsoft SQL Server, and employs the C# programming language for backend operations.

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


## Contributors

This project was developed by a dedicated team of students from the Department of Computer Engineering, Shah and Anchor Kutchhi Engineering College, under the guidance of Prof. Rupali Kale. The team members include:

- Sanal Pillai
- Sujith Nair
- Apresh Pandit
- Onkaar Sawant

## Future Scope

The project aims to extend its capabilities by incorporating more diverse datasets and improving the accuracy of sentiment analysis. Future developments will also explore fuzzy opinion classification to enhance the system's ability to understand and summarize user sentiments more effectively.

## License

This project is licensed under the MIT License - see the LICENSE.md file for details.

## Acknowledgements

- Our mentors and advisors for guidance
- The open-source community for various tools and libraries used

For detailed documentation and methodology, refer to our comprehensive report included in the repository.
