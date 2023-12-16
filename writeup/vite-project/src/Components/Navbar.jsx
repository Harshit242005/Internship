import React from 'react';
import { Link } from 'react-router-dom';

function Navbar({ userData, nickname }) {
    function arrayBufferToBase64(buffer) {
        let binary = '';
        const bytes = new Uint8Array(buffer);
        for (let i = 0; i < bytes.byteLength; i++) {
            binary += String.fromCharCode(bytes[i]);
        }
        return btoa(binary);
    }
    return (
        <nav className="navbar navbar-expand-lg bg-body-tertiary">
            <div className="container-fluid">
                <a className="navbar-brand" href="#">
                    {userData.Image && (
                        <img
                            className={styles.NavbarImage}
                            src={`data:image/png;base64,${arrayBufferToBase64(
                                userData.Image.data
                            )}`}
                            alt="User's Profile"
                        />
                    )}
                </a>
                <button
                    className="navbar-toggler"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#navbarSupportedContent"
                    aria-controls="navbarSupportedContent"
                    aria-expanded="false"
                    aria-label="Toggle navigation"
                >
                    <span className="navbar-toggler-icon"></span>
                </button>
                <div className="collapse navbar-collapse" id="navbarSupportedContent">
                    <ul className="navbar-nav me-auto mb-2 mb-lg-0">
                        <li className="nav-item">
                            <a className="nav-link active" aria-current="page" href="#">
                                Blogs
                            </a>
                        </li>
                        <li className="nav-item">
                            <Link className="nav-link" to={`/Create/${nickname}`}>
                                Create
                            </Link>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link" href="#">
                                Liked
                            </a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link" href="#">
                                Bookmarks
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    );
}

export default Navbar;
