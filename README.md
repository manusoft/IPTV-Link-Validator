# IPTV API

<p align="left"><img src="https://visitor-badge.laobi.icu/badge?page_id=manusoft.IPTVAPI" style="max-width: 100%;"></p>

This repository contains a WinUI3 application that fetches data from the [IPTV-ORG API](https://github.com/iptv-org/api) to validate stream quality and save channel and streams to a local database. The application provides functionality to validate streams, update their state in the database, and save all working channels and their streams to a JSON file, either by country or for all channels.

![111](https://github.com/manusoft/IPTVAPI/assets/83714923/47cfbf79-bb96-43e8-96fb-ab59a599fffa)
![1122](https://github.com/manusoft/IPTVAPI/assets/83714923/e6f1b5aa-1eeb-4420-9d11-fe2d2b5c8dcb)
![222](https://github.com/manusoft/IPTVAPI/assets/83714923/890d81dc-636b-4194-9c8b-feba81969f0a)
![333](https://github.com/manusoft/IPTVAPI/assets/83714923/8b9a389d-c724-466f-b745-338cd5a68172)
![4444](https://github.com/manusoft/IPTVAPI/assets/83714923/52420d28-f1f5-4289-bb68-1cf29e1c7d40)

## Features

- Fetches data from the IPTV-ORG API.
- Validates stream quality.
- Saves channels and streams to a local database.
- Updates stream states in the database.
- Saves working channels and their streams to a JSON file.

## Technologies Used

- WinUI3: The application is built using the WinUI3 framework.
- C#: The application is developed in C# programming language.
- SQLite: The local database is implemented using SQLite.
- IPTV-ORG API: The application fetches data from the IPTV-ORG API.

## Getting Started

To run the application locally, follow these steps:

1. Clone this repository to your local machine.
2. Open the solution file (`IPTVAPI.sln`) in Visual Studio.
3. Restore the NuGet packages used in the project.
4. Build the solution to restore dependencies and compile the application.
5. Run the application from Visual Studio.

## Usage

1. Upon launching the application, it will fetch data from the IPTV-ORG API.
2. The fetched channels and streams will be stored in the local database.
3. You can use the application to validate stream quality and update their states in the database.
4. To save all working channels and their streams to a JSON file, use the provided functionality.
5. Specify the country or select the option to save all channels.
6. The JSON file will be generated and saved locally.

## Contribution

Contributions to this repository are welcome. If you find any issues or have suggestions for improvements, please feel free to open an issue or submit a pull request.

When contributing to this repository, please keep the following guidelines in mind:

- Fork the repository and clone it locally.
- Create a new branch for your contribution.
- Commit your changes with clear and descriptive messages.
- Push your branch to your forked repository.
- Open a pull request, clearly describing the changes you have made.

## License

This project is licensed under the [MIT License](LICENSE).

## Legal

No video files are stored in this repository. The repository simply contains user-submitted links to publicly available video stream URLs, which to the best of our knowledge have been intentionally made publicly by the copyright holders. If any links in these playlists infringe on your rights as a copyright holder, they may be removed by sending a [pull request](https://github.com/iptv-org/database/pulls) or opening an [issue](https://github.com/iptv-org/database/issues/new/choose). However, note that we have **no control** over the destination of the link, and just removing the link from the playlist will not remove its contents from the web. Note that linking does not directly infringe copyright because no copy is made on the site providing the link, and thus this is **not** a valid reason to send a DMCA notice to GitHub. To remove this content from the web, you should contact the web host that's actually hosting the content (**not** GitHub, nor the maintainers of this repository).

## Acknowledgments

This application utilizes the IPTV-ORG API to fetch channel and stream data. We would like to express our gratitude to the developers and contributors of the IPTV-ORG API for providing this valuable resource.

## Contact

For any questions or inquiries, please contact the project maintainer:

- GitHub: [github.com/manusoft](https://github.com/manusoft)
