// license:BSD-3-Clause
// copyright-holders:Olivier Galibert,Aaron Giles
/*********************************************************/
/*    ricoh RF5C68(or clone) PCM controller              */
/*********************************************************/

#ifndef MAME_SOUND_RF5C68_H
#define MAME_SOUND_RF5C68_H

#pragma once


//**************************************************************************
//  TYPE DEFINITIONS
//**************************************************************************

#define RF5C68_SAMPLE_END_CB_MEMBER(_name)   void _name(int channel)


// ======================> rf5c68_device

class rf5c68_device : public device_t,
						public device_sound_interface,
						public device_memory_interface
{
public:
	typedef device_delegate<void (int channel)> sample_end_cb_delegate;

	rf5c68_device(const machine_config &mconfig, const char *tag, device_t *owner, uint32_t clock);

	template <typename... T> void set_end_callback(T &&... args) { m_sample_end_cb.set(std::forward<T>(args)...); }

	u8 rf5c68_r(offs_t offset);
	void rf5c68_w(offs_t offset, u8 data);

	u8 rf5c68_mem_r(offs_t offset);
	void rf5c68_mem_w(offs_t offset, u8 data);

	void map(address_map &map);
protected:
	rf5c68_device(const machine_config &mconfig, device_type type, const char *tag, device_t *owner, u32 clock);

	// device-level overrides
	virtual void device_start() override;
	virtual void device_clock_changed() override;

	// sound stream update overrides
	virtual void sound_stream_update(sound_stream &stream, stream_sample_t **inputs, stream_sample_t **outputs, int samples) override;

	// device_memory_interface configuration
	virtual space_config_vector memory_space_config() const override;

	address_space_config m_data_config;

private:
	static constexpr unsigned NUM_CHANNELS = 8;

	struct pcm_channel
	{
		pcm_channel() { }

		uint8_t       enable = 0;
		uint8_t       env    = 0;
		uint8_t       pan    = 0;
		uint8_t       start  = 0;
		uint32_t      addr   = 0;
		uint16_t      step   = 0;
		uint16_t      loopst = 0;
	};

	memory_access<16, 0, 0, ENDIANNESS_LITTLE>::cache m_cache;
	sound_stream*                                     m_stream;
	pcm_channel                                       m_chan[NUM_CHANNELS];
	uint8_t                                           m_cbank;
	uint16_t                                          m_wbank;
	uint8_t                                           m_enable;
	uint8_t											  m_pcmram[64*1024];

	sample_end_cb_delegate m_sample_end_cb;
};

class rf5c164_device : public rf5c68_device
{
public:
	rf5c164_device(const machine_config &mconfig, const char *tag, device_t *owner, uint32_t clock);
};

DECLARE_DEVICE_TYPE(RF5C68, rf5c68_device)
DECLARE_DEVICE_TYPE(RF5C164, rf5c164_device)

#endif // MAME_SOUND_RF5C68_H
