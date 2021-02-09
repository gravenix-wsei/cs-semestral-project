CREATE TABLE address(
	id_address INT CONSTRAINT pk_address PRIMARY KEY IDENTITY(1,1),
	address VARCHAR(255) NOT NULL,
	postcode VARCHAR(7) NOT NULL,
	city VARCHAR(255) NOT NULL,
	contact_phone VARCHAR(40) NOT NULL
);

CREATE TABLE hotel(
	hotel_id INT CONSTRAINT pk_hotel PRIMARY KEY IDENTITY(1,1),
	name VARCHAR(255) NOT NULL,
	description TEXT DEFAULT '',
	id_address INT CONSTRAINT ref_hotel_address REFERENCES address(id_address) NOT NULL,
	stars TINYINT CONSTRAINT hotel_stars CHECK(stars > 0 AND stars < 6) NOT NULL
);

CREATE TABLE room(
	room_id INT CONSTRAINT pk_room PRIMARY KEY IDENTITY(1,1),
	name VARCHAR(255) NOT NULL,
	description TEXT NOT NULL,
	size TINYINT NOT NULL,
	hotel_id INT CONSTRAINT ref_hotel REFERENCES hotel(hotel_id)
);

CREATE TABLE reservation(
	reservation_id INT CONSTRAINT pk_reservation PRIMARY KEY IDENTITY(1,1),
	room_id INT CONSTRAINT ref_reservation_room REFERENCES room(room_id),
	firstname VARCHAR(255) NOT NULL,
	lastname VARCHAR(255) NOT NULL,
	id_address_customer INT CONSTRAINT ref_reservation_address REFERENCES address(id_address),
	date_from DATE NOT NULL,
	date_to DATE NOT NULL
);
INSERT INTO address(address, postcode, city, contact_phone) VALUES('ul. Jarzębskiego 12', '31-231', 'Kraków', '+48 231 432 212');
INSERT INTO address(address, postcode, city, contact_phone) VALUES('ul. Wesoła 1', '31-230', 'Kraków', '+48 735 372 182');
INSERT INTO address(address, postcode, city, contact_phone) VALUES('ul. Wojtasa 13', '31-231', 'Kraków', '+48 652 823 912');

INSERT INTO hotel(name, description, id_address, stars) VALUES('Hotel Pod Wyrwygroszem', 'Miły, fajny, przyjemny hotel', 1, 3);
INSERT INTO hotel(name, description, id_address, stars) VALUES('Hotel Pomya', 'Miły, fajny, przyjemny hotel', 2, 3);

INSERT INTO room(name, description, hotel_id, size) VALUES('Pokoj mały', 'Mały i jednoosobowy pokój', 1, 1);
INSERT INTO room(name, description, hotel_id, size) VALUES('Pokoj mały 2', 'Mały i jednoosobowy pokój', 1, 1);
INSERT INTO room(name, description, hotel_id, size) VALUES('Pokoj mały 3', 'Mały i jednoosobowy pokój', 1, 1);
INSERT INTO room(name, description, hotel_id, size) VALUES('Pokoj dwuosobowy 3', 'Pokój na dwie osoby', 2, 1);

INSERT INTO room(name, description, hotel_id, size) VALUES('Pokoj mały ', 'Mały i jednoosobowy pokój', 1, 2);
INSERT INTO room(name, description, hotel_id, size) VALUES('Pokoj średni', 'Pokój na dwie osoby', 2, 2);

INSERT INTO reservation(room_id, firstname, lastname, id_address_customer, date_from, date_to) VALUES(1, 'Jan', 'Nowak', null, '2021-02-13', '2021-02-15');
INSERT INTO reservation(room_id, firstname, lastname, id_address_customer, date_from, date_to) VALUES(1, 'Jan', 'Nowak', null, '2021-03-15', '2021-03-16');
INSERT INTO reservation(room_id, firstname, lastname, id_address_customer, date_from, date_to) VALUES(5, 'Jakub', 'Kuśmider', null, '2021-02-13', '2021-02-14');